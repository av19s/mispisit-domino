using System;
using System.Collections.Generic;

namespace domino
{
    public class Controller
    {
        private readonly GamePull _game_pull;

        public Controller(GamePull game_pull)
        {
            _game_pull = game_pull;
        }

        public Game create_game()
        {
            return _game_pull.create_game();
        }

        public Game start_game(List<Player> players)
        {
            if (players == null || players.Count < 2 || players.Count > 4)
            {
                throw new ArgumentException("Количество игроков должно быть от 2 до 4");
            }

            if (players[0] == null ||
                players[1] == null ||
                object.ReferenceEquals(players[0], players[1]))
            {
                throw new ArgumentException(
                    "Необходимо передать двух разных игроков"
                );
            }

            Game game = create_game();

            TKoloda koloda = new TKoloda();
            Referee referee = new Referee();

            // Подключаем наблюдателя
            GameObserver observer = new GameObserver();

            game.BonePlayed += observer.OnBonePlayed;

            // Запускаем полный игровой цикл
            game.run_game(players, koloda, referee);

            return game;
        }
    }
}
