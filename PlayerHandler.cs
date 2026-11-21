using System;

namespace domino
{
    public class PlayerHandler
    {
        public Player Player { get; }

        // Следующий игрок (справа)
        public PlayerHandler? Successor { get; set; }

        // Предыдущий игрок (слева)
        public PlayerHandler? Previous { get; set; }

        // Удалось ли сделать ход
        public bool LastTurnSucceeded { get; private set; }

        public PlayerHandler(Player player)
        {
            Player = player ?? throw new ArgumentNullException(nameof(player));
        }

        // Выполнить один ход
        public void Handle(Game game, TKoloda koloda)
        {
            if (game == null || koloda == null)
            {
                throw new ArgumentNullException();
            }

            LastTurnSucceeded = false;

            if (TryMakeMove(game))
            {
                LastTurnSucceeded = true;
                return;
            }

            if (koloda.BoneCount > 0)
            {
                Player.TakeBone(koloda);
                Console.WriteLine($"{Player.Name} взял одну кость");

                if (TryMakeMove(game))
                {
                    LastTurnSucceeded = true;
                    return;
                }
            }

            SkipTurn();
        }

        private bool TryMakeMove(Game game)
        {
            for (int i = 0; i < Player.BoneCount; i++)
            {
                if (game.TryPlayBone(Player, i, false))
                {
                    return true;
                }

                if (game.TryPlayBone(Player, i, true))
                {
                    return true;
                }
            }

            return false;
        }


        // Пропуск хода
        private void SkipTurn()
        {
            LastTurnSucceeded = false;

            Console.WriteLine($"{Player.Name} пропускает ход");
        }
    }
}
