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

                IsEnded = enemy.CheckersCount == 0;
            }

            CurrentPlayerTurn = enemy;
        }
    }
}
