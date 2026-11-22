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
controller.start_game(new List<Player>
{
    player_1,
    player_2,
    player_3
});

Console.WriteLine("\nНажмите любую клавишу для завершения...");
Console.ReadKey();