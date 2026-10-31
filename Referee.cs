
using System;
using System.Collections.Generic;

namespace domino
{
    internal class Referee
    {
        // Метод для определения кто ходит первым
        public int WhoFirst(List<Player> players)
        {
            if (players == null || players.Count == 0)
            {
                throw new ArgumentException("Список игроков пуст");
            }

            int first_index = -1;
            int min_priority = int.MaxValue;
            int? first_double = null;

            // Ищем игрока с младшим дублем
            for (int i = 0; i < players.Count; i++)
            {
                int? player_double = players[i].GetMinDouble();

                if (player_double.HasValue)
                {
                    int priority = player_double.Value == 0 ? 7: player_double.Value;

                    if (priority < min_priority)
                    {
                        min_priority = priority;
                        first_double = player_double.Value;
                        first_index = i;
                    }
                }
            }

            // Если дубль найден, возвращается игрок
            if (first_index != -1)
            {
                Console.WriteLine(
                    $"Первым ходит {players[first_index].Name}, " +
                    $"дубль {first_double}:{first_double}"
                );

                return first_index;
            }

            // Если дублей нет, ищем игрока с минимальной суммой значений костей
            int min_sum = int.MaxValue;

            for (int i = 0; i < players.Count; i++)
            {
                int sum = players[i].GetMinSum();

                if (sum < min_sum)
                {
                    min_sum = sum;
                    first_index = i;
                }
            }

            Console.WriteLine(
                $"Первым ходит {players[first_index].Name}, " +
                $"минимальная сумма: {min_sum}"
            );

            return first_index;
        }
    }
}
