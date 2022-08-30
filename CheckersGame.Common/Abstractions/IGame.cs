using CheckersGame.Common.Abstractions.Board;

namespace CheckersGame.Common.Abstractions
{
    public interface IGame
    {
        string? EndMessage { get; }

        IBoard Board { get; init; }

        (IPlayer First, IPlayer Second) Players { get; init; }

        IPlayer CurrentPlayerTurn { get; }

        void NextTurn(Cell from, Cell to);
    }
}