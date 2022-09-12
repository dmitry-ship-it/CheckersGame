using CheckersGame.Common.Extensions;
using CheckersGame.Common.Impl.International;

var game = new InternationalGame();
PrintBoard();

// TODO: REFACTOR ALL THIS SHIT
while (true)
{
    Console.WriteLine($"Now its {game.CurrentPlayerTurn.Color.Name}'s turn:");

    Console.Write("From: ");
    var from = Console.ReadLine()!.Split(' ').Select(x => int.Parse(x));
    Console.WriteLine();

    Console.Write("To: ");
    var to = Console.ReadLine()!.Split(' ').Select(x => int.Parse(x));
    Console.WriteLine();

    Console.Clear();

    try
    {
        game.NextTurn(from.ToCell(), to.ToCell());
    }
    catch (Exception ex)
    {
        Console.WriteLine(ex.Message);
    }

    PrintBoard();
}

void PrintBoard()
{
    Console.Write("  ");

    for (int i = 0; i < game.Board.Cols; i++)
    {
        Console.Write($"{i} ");
    }

    Console.WriteLine();

    for (int i = 0; i < game.Board.Rows; i++)
    {
        Console.Write($"{i}|");

        for (int j = 0; j < game.Board.Cols; j++)
        {
            if (game.Board[i, j] is null)
            {
                Console.Write("  ");
            }

            if (game.Board[i, j]?.Color == game.Players.First.Color)
            {
                Console.Write(game.Board[i,j]!.ToString()!.StartsWith("Basic") ? "□ " : "◇ ");
            }

            if (game.Board[i, j]?.Color == game.Players.Second.Color)
            {
                Console.Write(game.Board[i, j]!.ToString()!.StartsWith("Basic") ? "■ " : "◆ ");
            }
        }

        Console.Write('|');
        Console.WriteLine();
    }
}
