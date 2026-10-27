using System;
using System.Collections.Generic;
using System.Text;

namespace domino
{

    public class Player
    {
        private Controller _controller;

        public Player(Controller controller)
        {
            _controller = controller;
        }

        public Game create_game()
        {
            return _controller.create_game();
        }
    }

}
