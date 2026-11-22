using System;
using System.Collections.Generic;

namespace domino
{
    public class Game
    {
        // Идентификатор игры
        public int Game_id { get; private set; }

        // Игроки и игровая цепочка
        private readonly List<Player> _players = new List<Player>();
        private TChain? _chain; // Игровая цепочка

        // Событие успешного хода
        public event Action<Player, TBone>? BonePlayed;

        public Game(int game_id)
        {
            Game_id = game_id;
        }

        public bool IsTableEmpty
        {
            get { return _chain == null; }
        }

        // Игровой цикл
        internal void run_game(List<Player> players, TKoloda koloda, Referee referee)
        {
            if (players == null || players.Count < 2 || players.Count > 4)
            {
                throw new ArgumentException("Количество игроков должно быть от 2 до 4");
            }

            if (koloda == null || referee == null)
            {
                throw new ArgumentNullException();
            }

            if (_players.Count > 0)
            {
                throw new InvalidOperationException("Игра уже запущена");
            }

            _players.AddRange(players);

            // Создаём кольцевую цепочку игроков
            referee.create_chain(players);

            // Раздаём каждому игроку по 7 костей
            foreach (Player player in players)
            {
                player.TakeBones(koloda, 7);
                player.ShowBones();
            }

            // Определяем первого игрока
            referee.choose_first_player();

            int consecutive_passes = 0;
            int turn_number = 1;

            while (true)
            {
                // Получаем текущего обработчика
                PlayerHandler current_handler = referee.CurrentHandler;
                Player current_player = current_handler.Player;

                Console.WriteLine($"\nХод {turn_number}: {current_player.Name}");

                // Выполняем ход
                current_handler.Handle(this, koloda);

                if (current_handler.LastTurnSucceeded)
                {
                    // Успешный ход сбрасывает счётчик пропусков
                    consecutive_passes = 0;

                    ShowTable();

                    // Проверяем победу
                    if (current_player.BoneCount == 0)
                    {
                        Console.WriteLine($"\nПобедитель: {current_player.Name}");
                        break;
                    }
                }
                else
                {
                    consecutive_passes++;

                    // Проверяем возникновение рыбы
                    if (consecutive_passes >= _players.Count &&
                        koloda.BoneCount == 0)
                    {
                        Console.WriteLine("\nРыба! Ходов больше нет.");

                        show_fish_result(referee);
                        break;
                    }
                }

                // Передаём ход следующему игроку
                referee.next_turn();

                turn_number++;
            }

            Console.WriteLine("\nИтоговые кости игроков:");

            foreach (Player player in _players)
            {
                player.ShowBones();
            }

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

        // Поиск подходящей кости и выполнение хода
        private bool try_play_any_bone(Player player)
        {
            for (int i = 0; i < player.BoneCount; i++)
            {
                // Пробуем поставить справа
                if (TryPlayBone(player, i, false))
                {
                    Console.WriteLine(
                        $"Кость с индексом {i} поставлена справа"
                    );

                    return true;
                }

                // Пробуем поставить слева
                if (TryPlayBone(player, i, true))
                {
                    Console.WriteLine(
                        $"Кость с индексом {i} поставлена слева"
                    );

                    return true;
                }
            }

            return false;
        }

        // Добор до появления подходящей кости
        private bool draw_until_playable(Player player, TKoloda koloda)
        {
            if (IsTableEmpty)
            {
                return false;
            }

            while (koloda.BoneCount > 0 &&
                   !player.HasPlayableBone(
                       _chain!.GetLeftEnd(),
                       _chain.GetRightEnd()))
            {
                player.TakeBone(koloda);

                Console.WriteLine(
                    $"{player.Name} взял кость из колоды"
                );
            }

            return player.HasPlayableBone(
                _chain!.GetLeftEnd(),
                _chain.GetRightEnd());
        }

        // Определение результата при рыбе
        private void show_fish_result(Referee referee)
        {
            foreach (Player player in _players)
            {
                Console.WriteLine(
                    $"{player.Name}: {player.GetRemainingPoints()} очков"
                );
            }

            int winner_index = referee.WhoWinsByPoints(_players);

            if (winner_index == -1)
            {
                Console.WriteLine("Ничья!");
            }
            else
            {
                Console.WriteLine(
                    $"Победитель: {_players[winner_index].Name}"
                );
            }
        }

        // Постановка кости на стол
        public bool TryPlayBone(
            Player player,
            int bone_index,
            bool to_left)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (!_players.Contains(player))
            {
                return false;
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

                BonePlayed?.Invoke(player, bone);

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

            // Удаляем кость только после успешного хода
            if (result)
            {
                player.RemoveBone(bone_index);

                BonePlayed?.Invoke(player, bone);
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
                $"Сыграно костей: {_chain.GetPlayedBoneCount()}"
            );
        }
    }
}
