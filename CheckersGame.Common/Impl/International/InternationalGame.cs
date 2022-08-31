using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using CheckersGame.Common.Impl.International.Board;
using System.Drawing;

namespace CheckersGame.Common.Impl.International
{
    public class InternationalGame : IGame
    {
        public IBoard Board { get; init; }

        public (IPlayer First, IPlayer Second) Players { get; init; }

        public IPlayer CurrentPlayerTurn { get; private set; }

        public string? EndMessage { get; private set; }

        public InternationalGame()
        {
            Board = new InternationalBoard();

            Players = (
                new InternationalPlayer(Color.White),
                new InternationalPlayer(Color.Black));

            CurrentPlayerTurn = Players.First;
        }

        public void NextTurn(Cell from, Cell to)
        {
            if (Board[from]?.Color != CurrentPlayerTurn.Color)
            {
                throw new ArgumentException("This player can't touch this checker.");
            }

            Board.HandleMove(from, to, out var isCheckerBeaten);

            var enemy = Players.First.Color == CurrentPlayerTurn.Color
                ? Players.Second
                : Players.First;

            if (isCheckerBeaten)
            {
                enemy.CheckersCount--;
                // add score??

                if (enemy.CheckersCount == 0)
                {
                    EndMessage = GetEndMessage();
                }
            }

            CurrentPlayerTurn = enemy;
        }

        private string GetEndMessage()
        {
            var checkersByCount = CurrentPlayerTurn.CheckersCount % 10 == 1
                ? "checker"
                : "checkers";

            return "Game ended. " +
                $"Winner is {CurrentPlayerTurn.Color} " +
                $"with {CurrentPlayerTurn.CheckersCount} " +
                $"{checkersByCount} remaining.";
        }
    }
}