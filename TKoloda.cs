
using System;
using System.Collections.Generic;

namespace domino
{
    public class TKoloda
    {
        Random rnd = new Random();

        private List<TBone> _dominos = new List<TBone>();

        public TKoloda()
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    TBone bone = new TBone(i, j);

                    _dominos.Add(bone);

                    Console.WriteLine($"{i}:{j}");
                }
            }
        }
        public TBone TakeBone()
        {
            if (_dominos.Count == 0)
            {
                throw new InvalidOperationException("Колода пуста");
            }

            int index = rnd.Next(0, _dominos.Count);

            TBone bone = _dominos[index];

            _dominos.RemoveAt(index);
            return bone;
        }
        public List<TBone> TakeBones(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }
            List<TBone> bones = new List<TBone>();

            for(int i =0; i < count; i++)
            {
                bones.Add(TakeBone());
            }
            return bones;
        }

        public void ShowPlayerBones(List<TBone> bones)
        {
            foreach(TBone bone in bones)
            {
                Console.WriteLine($"{bone.Left}:{bone.Right}");
            }
        }
        public void ShowRemainingBones()
        {
            Console.WriteLine("Оставшиеся кости в колоде:");

            if (_dominos.Count == 0)
            {
                Console.WriteLine("Колода пуста");
                return;
            }
            foreach (TBone domino in _dominos)
            {
                Console.WriteLine($"{domino.Left}:{domino.Right}");
            }
            Console.WriteLine($"Всего костей:{_dominos.Count}");
        }
    }
}
