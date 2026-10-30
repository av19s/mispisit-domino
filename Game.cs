using System;
using System.Collections.Generic;
using System.Text;

namespace domino
{
    public class Game
    {
        public int Game_id { get; private set; }
        private List<TBone> _table = new List<TBone>();
        private int _leftEnd;
        private int _rightEnd;

        public Game(int game_id)
        {
            Game_id = game_id;
        }
        public void ShowTable()
        {
            Console.WriteLine("Игровой стол:");
            if (_table.Count == 0)
            {
                Console.WriteLine("Стол пуст");
                return;
            }
            foreach (TBone bone in _table)
            {
                Console.Write($"[{bone.Left}:{bone.Right}]");
            }
            Console.WriteLine();
            Console.WriteLine($"Левый конец: {_leftEnd}");
            Console.WriteLine($"Правый конец: {_rightEnd}");
        }
        public bool IsTableEmpty
        {
            get { return _table.Count == 0; }
        }
        public bool TryPlayBone(Player player, int boneIndex, bool toLeft)
        {
            TBone bone = player.GetBone(boneIndex);

            if (IsTableEmpty)
            {
                _table.Add(bone);

                _leftEnd = bone.Left;
                _rightEnd = bone.Right;

                player.RemoveBone(boneIndex);

                return true;
            }
            if (toLeft)
            {
                if (bone.Right == _leftEnd)
                {
                    _leftEnd= bone.Left;
                }
                else if (bone.Left == _rightEnd)
                {
                    _leftEnd = bone.Right;
                }
                else
                {
                    return false;
                }
                _table.Insert(0, bone);

            }
            else
            {
                if (bone.Left == _rightEnd)
                {
                    _rightEnd= bone.Right;
                }
                else if(bone.Right == _rightEnd)
                {
                    _rightEnd = bone.Left;
                }
                else
                {
                    return false;
                }
                _table.Add(bone);
            }
            player.RemoveBone(boneIndex);
            return true;
        }
    }

}
