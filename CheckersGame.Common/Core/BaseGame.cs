using CheckersGame.Api.Core;
using System.Drawing;

namespace CheckersGame.Common.Core
{
    public sealed class BaseGame
    {
        public BaseGame(GameType gameType, GameRules gameRules)
        {
            GameType = gameType;
            GameRules = gameRules;

            Board = new BaseBoard(gameType, gameRules);

            Players = (
                new BasePlayer(Color.White, GameRules.CheckersCountForSinglePlayer),
                new BasePlayer(Color.Black, GameRules.CheckersCountForSinglePlayer));

            CurrentPlayerTurn = Players.First;
        }

        public BaseBoard Board { get; init; }

        public (BasePlayer First, BasePlayer Second) Players { get; init; }

        public BasePlayer CurrentPlayerTurn { get; private set; }

        public GameType GameType { get; init; }
        public GameRules GameRules { get; init; }

        public bool IsEnded { get; private set; }

        public void NextTurn(Cell[] cells)
        {
            if (cells.Length == 0)
            {
                throw new ArgumentException("Target cell not selected.", nameof(cells));
            }

            var targetCell = cells[0];
            if (Board[targetCell]?.Color != CurrentPlayerTurn.Color)
            {
                throw new ArgumentException("This player can't touch this checker.");
            }

            var checkersBeaten = Board.HandleMove(cells);

            var enemy = Players.First.Color == CurrentPlayerTurn.Color
                ? Players.Second
                : Players.First;

            enemy.CheckersCount -= checkersBeaten;
            // add score??

            IsEnded = enemy.CheckersCount == 0;

            CurrentPlayerTurn = enemy;
        }
    }
}
