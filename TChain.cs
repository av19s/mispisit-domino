
using System;

namespace domino
{
    public class TChain
    {
        // Стартовая кость
        private readonly TBone _start_bone;

        // Кости, добавленные слева и справа
        private readonly TBone[] _left_bones = new TBone[27];
        private readonly TBone[] _right_bones = new TBone[27];

        // Количество костей с каждой стороны
        private int _left_count = 0;
        private int _right_count = 0;

        // Конструктор цепочки
        public TChain(TBone start_bone)
        {
            _start_bone = start_bone ?? throw new ArgumentNullException(nameof(start_bone));
        }

        // Получение левого края
        public int GetLeftEnd()
        {
            if (_left_count == 0)
            {
                return _start_bone.Left;
            }

            return _left_bones[_left_count - 1].Left;
        }

        // Получение правого края
        public int GetRightEnd()
        {
            if (_right_count == 0)
            {
                return _start_bone.Right;
            }

            return _right_bones[_right_count - 1].Right;
        }

        // Добавление кости слева
        public bool AddLeft(TBone bone)
        {
            if (bone == null)
            {
                throw new ArgumentNullException(nameof(bone));
            }

            if (GetPlayedBoneCount() >= 28)
            {
                return false;
            }

            int left_end = GetLeftEnd();
            TBone table_bone = bone;

            if (bone.Right == left_end)
            {
                // Кость подходит без разворота
            }
            else if (bone.Left == left_end)
            {
                table_bone = new TBone(bone.Right, bone.Left);
            }
            else
            {
                return false;
            }

            _left_bones[_left_count] = table_bone;
            _left_count++;

            return true;
        }

        // Добавление кости справа
        public bool AddRight(TBone bone)
        {
            if (bone == null)
            {
                throw new ArgumentNullException(nameof(bone));
            }

            if (GetPlayedBoneCount() >= 28)
            {
                return false;
            }

            int right_end = GetRightEnd();
            TBone table_bone = bone;

            if (bone.Left == right_end)
            {
                // Кость подходит без разворота
            }
            else if (bone.Right == right_end)
            {
                table_bone = new TBone(bone.Right, bone.Left);
            }
            else
            {
                return false;
            }

            _right_bones[_right_count] = table_bone;
            _right_count++;

            return true;
        }

        // Количество костей, выложенных на стол
        public int GetPlayedBoneCount()
        {
            return 1 + _left_count + _right_count;
        }

        // Получение всех сыгранных костей
        public TBone[] GetPlayedBones()
        {
            TBone[] played_bones = new TBone[GetPlayedBoneCount()];

            int index = 0;

            // Левую часть читаем в обратном порядке
            for (int i = _left_count - 1; i >= 0; i--)
            {
                played_bones[index] = _left_bones[i];
                index++;
            }

            // Стартовая кость
            played_bones[index] = _start_bone;
            index++;

            // Правая часть
            for (int i = 0; i < _right_count; i++)
            {
                played_bones[index] = _right_bones[i];
                index++;
            }

            return played_bones;
        }
    }
}
