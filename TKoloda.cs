
using System;
using System.Collections.Generic;

namespace domino
{
    public class TKoloda
    {
        // Колода домино
        private readonly Random _rnd = new Random();

        // Список всех костей в колоде
        private readonly List<TBone> _dominos = new List<TBone>();

        // Конструктор класса TKoloda
        public TKoloda()
        {
            for (int i = 0; i <= 6; i++)
            {
                for (int j = i; j <= 6; j++)
                {
                    TBone bone = new TBone(i, j);

                    _dominos.Add(bone);
                }
            }
        }

        // Метод для взятия кости из колоды
        public TBone TakeBone()
        {
            if (_dominos.Count == 0)
            {
                throw new InvalidOperationException("Колода пуста");
            }

            int index = _rnd.Next(0, _dominos.Count);

            TBone bone = _dominos[index];

            _dominos.RemoveAt(index);

            return bone;
        }

        // Метод для взятия нескольких костей из колоды
        public List<TBone> TakeBones(int count)
        {
            if (count < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count));
            }

            if (count > _dominos.Count)
            {
                throw new InvalidOperationException("Недостаточно костей в колоде");
            }

            List<TBone> bones = new List<TBone>();

            for (int i = 0; i < count; i++)
            {
                bones.Add(TakeBone());
            }

            return bones;
        }

        // Метод для отображения оставшихся костей в колоде
        public void ShowRemainingBones()
        {
            Console.WriteLine("Оставшиеся кости в колоде:");

            if (_dominos.Count == 0)
            {
                Console.WriteLine("Колода пуста");
                return;
            }

            foreach (TBone bone in _dominos)
            {
                Console.WriteLine($"{bone.Left}:{bone.Right}");
            }
            
            Console.WriteLine($"Всего костей: {_dominos.Count}");
        }
    }
}
