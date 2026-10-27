using System;
using System.Collections.Generic;
using System.Text;

namespace domino
{

    public class Player
    {
        private Controller _controller;
        
        public string Name { get; private set; }
        private List<TBone>_bones = new List<TBone>();
        public Player(Controller controller, string name)
        {
            _controller = controller;
            Name = name;
        }

        public Game create_game()
        {
            return _controller.create_game();
        }
        public void TakeBone(TKoloda koloda)
        {
            TBone bone = koloda.TakeBone();
            _bones.Add(bone);
        }
        public void TakeBones(TKoloda koloda,int count)
        {
            _bones.AddRange(koloda.TakeBones(count));
        }
        public void ShowBones(TKoloda koloda)
        {
            Console.WriteLine($"Кости игрока {Name}:");
            koloda.ShowPlayerBones(_bones);
        }
    }

}
