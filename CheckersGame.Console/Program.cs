using CheckersGame.Common.Implementations.International;

var game = new Game();
PrintBoard();

// TODO: REFACTOR ALL THIS SHIT
while (true)
{
    Console.WriteLine($"Now its {game.CurrentPlayerTurn.Color}'s turn:");

    Console.Write("From: ");
    var from = Console.ReadLine()!.Split(' ').Select(x => int.Parse(x));
    Console.WriteLine();

    Console.Write("To: ");
    var to = Console.ReadLine()!.Split(' ').Select(x => int.Parse(x));
    Console.WriteLine();

    Console.Clear();

    try
    {
        game.NextTurn((from.First(), from.Last()), (to.First(), to.Last()));
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
            if (game.Board[i, j]?.Checker is null)
            {
                Console.Write("  ");
            }

            if (game.Board[i, j]?.Checker?.Color == game.FirstPlayer.Color)
            {
                Console.Write(game.Board[i,j].Checker?.ToString() == "Basic" ? "□ " : "◇ ");
            }

            if (game.Board[i, j]?.Checker?.Color == game.SecondPlayer.Color)
            {
                Console.Write(game.Board[i, j].Checker?.ToString() == "Basic" ? "■ " : "◆ ");
            }
        }

        Console.Write('|');
        Console.WriteLine();
    }
}
