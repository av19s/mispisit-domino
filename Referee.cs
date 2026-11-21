
using System;
using System.Collections.Generic;

namespace domino
{
    internal class Referee
    {

        private readonly List<PlayerHandler> _handlers = new List<PlayerHandler>();
        private PlayerHandler? _current_handler;

        // Создать кольцевую цепочку игроков
        public void create_chain(List<Player> players)
        {
            if (players == null || players.Count < 2 || players.Count > 4)
            {
                throw new ArgumentException("Количество игроков должно быть от 2 до 4");
            }

            HashSet<Player> unique_players = new HashSet<Player>();

            foreach (Player player in players)
            {
                if (player == null || !unique_players.Add(player))
                {
                    throw new ArgumentException("Игроки не должны повторяться");
                }
            }

            _handlers.Clear();

            foreach (Player player in players)
            {
                _handlers.Add(new PlayerHandler(player));
            }

            for (int i = 0; i < _handlers.Count; i++)
            {
                _handlers[i].Successor = _handlers[(i + 1) % _handlers.Count];
                _handlers[i].Previous = _handlers[(i - 1 + _handlers.Count) % _handlers.Count];
            }

            _current_handler = _handlers[0];
        }

        // Текущий игрок
        public PlayerHandler CurrentHandler
        {
            get
            {
                return _current_handler
                    ?? throw new InvalidOperationException("Цепочка игроков не создана");
            }
        }

        // Передача хода следующему игроку
        public void next_turn()
        {
            _current_handler = CurrentHandler.Successor!;
        }

        // Отображение расположения игроков
        public void show_seating()
        {
            foreach (PlayerHandler handler in _handlers)
            {
                Console.WriteLine(
                    $"{handler.Player.Name}: " +
                    $"слева – {handler.Previous!.Player.Name}, " +
                    $"справа – {handler.Successor!.Player.Name}"
                );
            }
        }

        // Метод для определения кто ходит первым
        internal int WhoFirst(List<Player> players)
        {
            if (players == null || players.Count < 2 || players.Count > 4)
            {
                throw new ArgumentException("Количество игроков должно быть от 2 до 4");
            }

            int first_index = -1;
            int min_priority = int.MaxValue;

            // Ищем младший дубль среди всех игроков
            for (int i = 0; i < players.Count; i++)
            {
                int? min_double = players[i].GetMinDouble();

                if (min_double == null)
                {
                    continue;
                }

                int priority = min_double.Value == 0 ? 7 : min_double.Value;

                if (priority < min_priority)
                {
                    min_priority = priority;
                    first_index = i;
                }
            }

            if (first_index != -1)
            {
                return first_index;
            }

            // Если дублей нет, ищем минимальную сумму кости
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

            return first_index;
        }

        // Метод для определения победителя по очкам (переделать)
        public int WhoWinsByPoints(List<Player> players)
        {
            if (players == null || players.Count != 2)
            {
                throw new ArgumentException("Необходимо два игрока");
            }

            int points_1 = players[0].GetRemainingPoints();
            int points_2 = players[1].GetRemainingPoints();

            if (points_1 == points_2)
            {
                return -1;
            }

            return points_1 < points_2 ? 0 : 1;
        }

    }
}
