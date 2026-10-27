using System;
using System.Collections.Generic;
using System.Text;

namespace domino
{
    public class GamePull
    {
        private List<Game> _games = new List<Game>();

        public Game create_game()
        {
            int game_id = _games.Count + 1;

            Game game = new Game(game_id);

            _games.Add(game);

            return game;
        }
    }
}
