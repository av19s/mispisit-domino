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

        // Создание новой игры
        public Game create_game()
        {
            return _game_pull.create_game();
        }

        // Подготовка и запуск игры
        public Game start_game(List<Player> players)
        {
            if (players == null || players.Count != 2)
            {
                throw new ArgumentException("Для игры необходимо два игрока");
            }

            if (players[0] == null || players[1] == null || object.ReferenceEquals(players[0], players[1]))
            {
                throw new ArgumentException("Необходимо передать двух разных игроков");
            }

            Game game = create_game();

            TKoloda koloda = new TKoloda();

            Referee referee = new Referee();

            // Создание наблюдателя
            GameObserver observer = new GameObserver();

            // Подписка на событие успешного хода
            game.BonePlayed += observer.OnBonePlayed;

            // Запуск демонстрационной партии
            game.start_demo(players, koloda, referee);

            return game;
        }
    }
}
