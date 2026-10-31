
using System;
using System.Collections.Generic;

namespace domino
{
    public class Game
    {
        // Уникальный идентификатор игры
        public int Game_id { get; private set; }

        private readonly List<TBone> _table = new List<TBone>();
        
        // players
        private readonly List<Player> _players = new List<Player>();

        // Левый и правый концы игрового стола
        private int _left_end;
        private int _right_end;

        // Конструктор класса Game
        public Game(int game_id)
        {
            Game_id = game_id;
        }

        // Метод для проверки, пуст ли игровой стол
        public bool IsTableEmpty
        {
            get { return _table.Count == 0; }
        }

        // Метод для получения текущего состояния игрового стола
        public void ShowTable()
        {
            Console.WriteLine("Игровой стол:");

            if (IsTableEmpty)
            {
                Console.WriteLine("Стол пуст");
                return;
            }

            foreach (TBone bone in _table)
            {
                Console.Write($"[{bone.Left}:{bone.Right}]");
            }

            Console.WriteLine();

            Console.WriteLine($"Левый конец: {_left_end}");
            Console.WriteLine($"Правый конец: {_right_end}");
        }

        // Метод для попытки сыграть кость игроком
        public bool TryPlayBone(Player player, int bone_index, bool to_left)
        {
            if (player == null)
            {
                throw new ArgumentNullException(nameof(player));
            }

            if (bone_index < 0 || bone_index >= player.BoneCount)
            {
                return false;
            }

            TBone bone = player.GetBone(bone_index);

            if (IsTableEmpty)
            {
                _table.Add(bone);

                _left_end = bone.Left;
                _right_end = bone.Right;

                player.RemoveBone(bone_index);

                return true;
            }

            TBone table_bone = bone;

            if (to_left)
            {
                if (bone.Right == _left_end)
                {
                    _left_end = bone.Left;
                }
                else if (bone.Left == _left_end)
                {
                    table_bone = new TBone(bone.Right, bone.Left);

                    _left_end = bone.Right;
                }
                else
                {
                    return false;
                }

                _table.Insert(0, table_bone);
            }
            else
            {
                if (bone.Left == _right_end)
                {
                    _right_end = bone.Right;
                }
                else if (bone.Right == _right_end)
                {
                    table_bone = new TBone(bone.Right, bone.Left);

                    _right_end = bone.Left;
                }
                else
                {

                    return false;
                }

                _table.Add(table_bone);
            }

            player.RemoveBone(bone_index);

            return true;
        }
        
    }
}
