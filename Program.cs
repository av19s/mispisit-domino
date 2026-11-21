using System.Collections.Generic;
using domino;

// Создание хранилища игр и контроллера
GamePull game_pull = new GamePull();

Controller controller = new Controller(game_pull);

// Создание игроков
Player player_1 = new Player(controller, "Матвей");
Player player_2 = new Player(controller, "Артём");
Player player_3 = new Player(controller, "Юра");

// Запуск игры
//controller.start_game(new List<Player>
//{
//    player_1,
//    player_2
//});

List<Player> players = new List<Player>
{
    player_1,
    player_2,
    player_3,
};

Referee referee = new Referee();

referee.create_chain(players);
referee.show_seating();

for (int i = 0; i < 6; i++)
{
    Console.WriteLine(
        $"Ход {i + 1}: {referee.CurrentHandler.Player.Name}"
    );

    referee.next_turn();
}

Console.WriteLine("\nНажмите любую клавишу для завершения...");
Console.ReadKey();