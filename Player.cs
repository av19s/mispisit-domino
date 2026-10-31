
using System;
using System.Collections.Generic;

namespace domino
{
    public class Player
    {
        // Контроллер для взаимодействия с игрой
        private readonly Controller _controller;

        // Список костей игрока
        private readonly List<TBone> _bones = new List<TBone>();

        // Имя игрока
        public string Name { get; private set; }

        // Конструктор класса Player
        public Player(Controller controller, string name)
        {
            _controller = controller;
            Name = name;
        }

        // Метод для создания новой игры
        public Game create_game()
        {
            return _controller.create_game();
        }

        // Метод для взятия кости из колоды
        public void TakeBone(TKoloda koloda)
        {
            TBone bone = koloda.TakeBone();

            _bones.Add(bone);
        }

        // Метод для получения минимальной суммы значений костей игрока
        public int GetMinSum()
        {
            int minSum = int.MaxValue;

            foreach(TBone bone in _bones)
            {
                if (bone.Left + bone.Right < minSum)
                {
                    minSum = bone.Left + bone.Right;
                }
            }

            return minSum;
        }

        // Метод для получения минимального дубля игрока
        public int? GetMinDouble()
        {
            int? min_double = null;
            int min_priority = int.MaxValue;

            foreach (TBone bone in _bones)
            {
                if (bone.Left == bone.Right)
                {
                    int priority = bone.Left == 0 ? 7 : bone.Left;

                    if (priority < min_priority)
                    {
                        min_priority = priority;
                        min_double = bone.Left;
                    }
                }
            }

            return min_double;
        }


        // Метод для взятия нескольких костей из колоды
        public void TakeBones(TKoloda koloda, int count)
        {
            _bones.AddRange(koloda.TakeBones(count));
        }

        // Метод для отображения костей игрока
        public void ShowBones()
        {
            Console.WriteLine($"Кости игрока {Name}:");

            foreach (TBone bone in _bones)
            {
                Console.WriteLine($"{bone.Left}:{bone.Right}");
            }
        }

        // Метод для получения количества костей у игрока
        public int BoneCount
        {
            get { return _bones.Count; }
        }

        // Метод для получения кости по индексу
        public TBone GetBone(int index)
        {
            return _bones[index];
        }

        // Метод для удаления кости по индексу
        public TBone RemoveBone(int index)
        {
            TBone bone = _bones[index];

            _bones.RemoveAt(index);

            return bone;
        }

        // Метод для проверки наличия кости, которую можно сыграть
        public bool HasPlayableBone(int left_end, int right_end)
        {
            foreach (TBone bone in _bones)
            {
                if (bone.Left == left_end ||
                    bone.Right == left_end ||
                    bone.Left == right_end ||
                    bone.Right == right_end)
                {
                    return true;
                }
            }

            return false;
        }
    }
}
