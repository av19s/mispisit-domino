
using System;
using System.Collections.Generic;
using domino;


GamePull game_pull = new GamePull();
Controller controller = new Controller(game_pull);


Player player_1 = new Player(controller, "Матвей");
Player player_2 = new Player(controller, "Артём");

Referee referee = new Referee();


Game game = player_1.create_game();

Console.WriteLine($"Создана игра №{game.Game_id}");


TKoloda koloda = new TKoloda();


player_1.TakeBones(koloda, 7);
player_2.TakeBones(koloda, 7);


Console.WriteLine("\nКости игроков:");

player_1.ShowBones();
player_2.ShowBones();


List<Player> players = new List<Player>
{
    player_1,
    player_2
};

int first_index = referee.WhoFirst(players);

Player first_player = players[first_index];
Player second_player = players[1 - first_index];

Console.WriteLine($"\nПервым ходит: {first_player.Name}");
Console.WriteLine($"Вторым ходит: {second_player.Name}");


int first_bone_index = -1;

int? min_double = first_player.GetMinDouble();

for (int i = 0; i < first_player.BoneCount; i++)
{
    TBone bone = first_player.GetBone(i);

    if (min_double.HasValue)
    {
        if (bone.Left == min_double.Value &&
            bone.Right == min_double.Value)
        {
            first_bone_index = i;
            break;
        }
    }
    else
    {
        if (bone.Left + bone.Right == first_player.GetMinSum())
        {
            first_bone_index = i;
            break;
        }
    }
}


Console.WriteLine($"\n{first_player.Name} делает первый ход:");

bool move_1 = game.TryPlayBone(first_player, first_bone_index, true);

Console.WriteLine($"Результат хода: {move_1}");

game.ShowTable();


Console.WriteLine($"\n{second_player.Name} делает ход:");

bool move_2 = false;

for (int i = 0; i < second_player.BoneCount; i++)
{
    if (game.TryPlayBone(second_player, i, false))
    {
        move_2 = true;

        Console.WriteLine($"Кость с индексом {i} поставлена справа");

        break;
    }

    if (game.TryPlayBone(second_player, i, true))
    {
        move_2 = true;

        Console.WriteLine($"Кость с индексом {i} поставлена слева");

        break;
    }
}

if (!move_2)
{
    Console.WriteLine("У второго игрока нет подходящей кости");
}

Console.WriteLine($"Результат хода: {move_2}");

game.ShowTable();

Console.WriteLine("\nКости игроков после ходов:");

player_1.ShowBones();
player_2.ShowBones();


Console.WriteLine("\nОстаток колоды:");

koloda.ShowRemainingBones();
