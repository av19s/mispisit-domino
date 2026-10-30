
using domino;

//GamePull game_pull = new GamePull();

//Controller controller = new Controller(game_pull);

//Player player = new Player(controller,"Matvey");

////Game game_1 = player.create_game();
////Game game_2 = player.create_game();
////Game game_3 = player.create_game();

////Console.WriteLine($"Игра №{game_1.Game_id} создана");
////Console.WriteLine($"Игра №{game_2.Game_id} создана");
////Console.WriteLine($"Игра №{game_3.Game_id} создана");

//TKoloda koloda = new TKoloda();

////TBone bone = koloda.TakeBone();
////TBone bone2 = koloda.TakeBone();
//player.TakeBones(koloda, 7);
//player.ShowBones(koloda);
//koloda.ShowRemainingBones();

Game game = new Game(1);

GamePull gamePull = new GamePull();
Controller controller = new Controller(gamePull);

Player player1 = new Player(controller, "Матвей");
Player player2 = new Player(controller, "Игрок 2");

TKoloda koloda = new TKoloda();

player1.TakeBones(koloda, 7);
player2.TakeBones(koloda, 7);

Console.WriteLine("\nКости первого игрока:");
player1.ShowBones(koloda);

Console.WriteLine("\nКости второго игрока:");
player2.ShowBones(koloda);

Console.WriteLine("\nПервый игрок делает ход:");

bool move1 = game.TryPlayBone(player1, 0, true);
Console.WriteLine($"Результат хода: {move1}");

game.ShowTable();

Console.WriteLine("\nВторой игрок делает ход:");

bool move2 = game.TryPlayBone(player2, 0, false);
Console.WriteLine($"Результат хода: {move2}");

game.ShowTable();

Console.WriteLine("\nОстаток колоды:");
koloda.ShowRemainingBones();