using System;
using System.Collections.Generic;
using System.Text;

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
    }
}
