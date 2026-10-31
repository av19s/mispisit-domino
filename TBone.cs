using System;
using System.Collections.Generic;
using System.Text;

namespace domino
{
    public class TBone
    {
        public int Left { get; private set; }
        public int Right { get; private set; }

        // Конструктор класса TBone
        public TBone(int left, int right)
        {
            Left = left;
            Right = right;
        }
    }
}
