using System;

namespace domino
{
    public class GameObserver
    {
        // Обработка события успешного хода
        public void OnBonePlayed(Player player, TBone bone)
        {
            Console.WriteLine(
                $"Наблюдатель: {player.Name} сыграл костью " +
                $"[{bone.Left}:{bone.Right}]"
            );
        }
    }
}
