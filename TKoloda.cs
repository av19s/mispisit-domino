
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

            Console.WriteLine($"Взята кость: {bone.Left}:{bone.Right}");

            Console.WriteLine("Оставшиеся кости:");

            foreach (TBone domino in _dominos)
            {
                Console.WriteLine($"{domino.Left}:{domino.Right}");
            }

            return bone;
        }

    }
}
