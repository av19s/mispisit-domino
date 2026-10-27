using System;
using System.Collections.Generic;
using System.Text;

namespace domino
{
    public class Game
    {
        public int Game_id { get; private set; }

        public Game(int game_id)
        {
            Game_id = game_id;
        }
    }

}
