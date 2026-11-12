using System;
using System.Collections.Generic;

namespace domino
{
    public class Game
    {
        public int Game_id { get; private set; }

        private readonly List<TBone> _table = new List<TBone>();
        private readonly List<Player> _players = new List<Player>();

        private int _left_end;
        private int _right_end;

        public Game(int game_id)
        {
            Game_id = game_id;
        }

        public bool IsTableEmpty
        {
            get { return _table.Count == 0; }
        }

        // Запуск партии
        internal void start_demo(
            List<Player> players,
            TKoloda koloda,
            Referee referee)
        {
            if (_players.Count > 0)
            {
                throw new InvalidOperationException("Игра уже запущена");
            }

            _players.AddRange(players);

            Console.WriteLine($"Создана игра №{Game_id}");

            // Раздача костей
            foreach (Player player in _players)
            {
                player.TakeBones(koloda, 7);
            }

            Console.WriteLine("\nКости игроков:");

            foreach (Player player in _players)
            {
                player.ShowBones();
            }

            // Определение первого игрока
            int first_index = referee.WhoFirst(_players);

            Player first_player = _players[first_index];
            Player second_player = _players[1 - first_index];

            Console.WriteLine($"\nПервым ходит: {first_player.Name}");
            Console.WriteLine($"Вторым ходит: {second_player.Name}");

            // Поиск стартовой кости
            int first_bone_index = find_first_bone_index(first_player);

            if (first_bone_index == -1)
            {
                throw new InvalidOperationException("Стартовая кость не найдена");
            }

            // Первый ход
            Console.WriteLine($"\n{first_player.Name} делает первый ход:");

            bool move_1 = TryPlayBone(first_player, first_bone_index, true);

            Console.WriteLine($"Результат хода: {move_1}");

            ShowTable();

            // Второй ход
            Console.WriteLine($"\n{second_player.Name} делает ход:");

            bool move_2 = try_play_any_bone(second_player);

            if (!move_2)
            {
                Console.WriteLine("У второго игрока нет подходящей кости");
            }

            Console.WriteLine($"Результат хода: {move_2}");

            ShowTable();

            // Вывод результатов
            Console.WriteLine("\nКости игроков после ходов:");

            foreach (Player player in _players)
            {
                player.ShowBones();
            }

            Console.WriteLine("\nОстаток колоды:");

            koloda.ShowRemainingBones();
        }

        // Поиск стартовой кости игрока
        private int find_first_bone_index(Player player)
        {
            int? min_double = player.GetMinDouble();
            int min_sum = player.GetMinSum();

            for (int i = 0; i < player.BoneCount; i++)
            {
                TBone bone = player.GetBone(i);

                if (min_double.HasValue)
                {
                    if (bone.Left == min_double.Value &&
                        bone.Right == min_double.Value)
                    {
                        return i;
                    }
                }
                else
                {
                    if (bone.Left + bone.Right == min_sum)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        // Поиск и выполнение допустимого хода
        private bool try_play_any_bone(Player player)
        {
            for (int i = 0; i < player.BoneCount; i++)
            {
                if (TryPlayBone(player, i, false))
                {
                    Console.WriteLine($"Кость поставлена справа");
                    return true;
                }

                if (TryPlayBone(player, i, true))
                {
                    Console.WriteLine($"Кость поставлена слева");
                    return true;
                }
            }

            return false;
        }

        // Отображение игрового стола
        public void ShowTable()
        {
            Console.WriteLine("Игровой стол:");

            if (IsTableEmpty)
            {
                Console.WriteLine("Стол пуст");
                return;
            }

            foreach (TBone bone in _table)
            {
                Console.Write($"[{bone.Left}:{bone.Right}]");
            }

            Console.WriteLine();

            Console.WriteLine($"Левый конец: {_left_end}");
            Console.WriteLine($"Правый конец: {_right_end}");
        }

        // Попытка поставить кость на стол
        public bool TryPlayBone(Player player, int bone_index, bool to_left)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (bone_index < 0 || bone_index >= player.BoneCount)
            {
                return false;
            }

            TBone bone = player.GetBone(bone_index);

            if (IsTableEmpty)
            {
                _table.Add(bone);

                _left_end = bone.Left;
                _right_end = bone.Right;

                player.RemoveBone(bone_index);

                return true;
            }

            TBone table_bone = bone;

            if (to_left)
            {
                if (bone.Right == _left_end)
                {
                    _left_end = bone.Left;
                }
                else if (bone.Left == _left_end)
                {
                    table_bone = new TBone(bone.Right, bone.Left);

                    _left_end = bone.Right;
                }
                else
                {
                    return false;
                }

                _table.Insert(0, table_bone);
            }
            else
            {
                if (bone.Left == _right_end)
                {
                    _right_end = bone.Right;
                }
                else if (bone.Right == _right_end)
                {
                    table_bone = new TBone(bone.Right, bone.Left);

                    _right_end = bone.Left;
                }
                else
                {
                    return false;
                }

                _table.Add(table_bone);
            }

            player.RemoveBone(bone_index);

            return true;
        }
    }
}
