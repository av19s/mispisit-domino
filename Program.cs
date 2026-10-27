
using domino;

GamePull game_pull = new GamePull();

Controller controller = new Controller(game_pull);

Player player = new Player(controller,"Matvey");

//Game game_1 = player.create_game();
//Game game_2 = player.create_game();
//Game game_3 = player.create_game();

//Console.WriteLine($"Игра №{game_1.Game_id} создана");
//Console.WriteLine($"Игра №{game_2.Game_id} создана");
//Console.WriteLine($"Игра №{game_3.Game_id} создана");

TKoloda koloda = new TKoloda();

//TBone bone = koloda.TakeBone();
//TBone bone2 = koloda.TakeBone();
player.TakeBones(koloda, 7);
player.ShowBones(koloda);
koloda.ShowRemainingBones();