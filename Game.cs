
using System;
using System.Collections.Generic;

namespace domino
{
    public class Game
    {
        // Идентификатор игры
        public int Game_id { get; private set; }

        // Игроки
        private readonly List<Player> _players = new List<Player>();

        // Игровая цепочка
        private TChain? _chain;

        // Конструктор
        public Game(int game_id)
        {
            Game_id = game_id;
        }

        // Проверка пустого стола
        public bool IsTableEmpty
        {
            get { return _chain == null; }
        }

        // Запуск демонстрационной игры
        internal void start_demo(
            List<Player> players,
            TKoloda koloda,
            Referee referee)
        {
            if (players == null || players.Count != 2)
            {
                throw new ArgumentException("Необходимо два игрока");
            }

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
                Console.WriteLine("У игрока нет подходящей кости");
            }

            Console.WriteLine($"Результат хода: {move_2}");

            ShowTable();

            // Состояние игроков
            Console.WriteLine("\nКости игроков после ходов:");

            foreach (Player player in _players)
            {
                player.ShowBones();
            }

            // Оставшаяся колода
            Console.WriteLine("\nОстаток колоды:");

            koloda.ShowRemainingBones();
        }

        // Поиск стартовой кости
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

        // Поиск подходящей кости
        private bool try_play_any_bone(Player player)
        {
            for (int i = 0; i < player.BoneCount; i++)
            {
                if (TryPlayBone(player, i, false))
                {
                    Console.WriteLine($"Кость {i} поставлена справа");
                    return true;
                }

                if (TryPlayBone(player, i, true))
                {
                    Console.WriteLine($"Кость {i} поставлена слева");
                    return true;
                }
            }

            return false;
        }

        // Выполнение хода
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

            // Первая кость создаёт цепочку
            if (IsTableEmpty)
            {
                _chain = new TChain(bone);

                player.RemoveBone(bone_index);

                return true;
            }

            // Добавление в существующую цепочку
            bool result;

            if (to_left)
            {
                result = _chain!.AddLeft(bone);
            }
            else
            {
                result = _chain!.AddRight(bone);
            }

            // Удаляем кость из руки только при успешном ходе
            if (result)
            {
                player.RemoveBone(bone_index);
            }

            return result;
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

            foreach (TBone bone in _chain!.GetPlayedBones())
            {
                Console.Write($"[{bone.Left}:{bone.Right}]");
            }

            Console.WriteLine();

            Console.WriteLine($"Левый конец: {_chain.GetLeftEnd()}");
            Console.WriteLine($"Правый конец: {_chain.GetRightEnd()}");

            Console.WriteLine(
                $"Количество сыгранных костей: {_chain.GetPlayedBoneCount()}"
            );
        }
    }
}
