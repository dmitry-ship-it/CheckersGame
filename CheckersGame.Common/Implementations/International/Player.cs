using CheckersGame.Common.Abstractions;
using CheckersGame.Common.Abstractions.Board;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International
{
    internal class Player : IPlayer
    {
        private readonly IBoard _board;

        public Player(IBoard board, Color color)
        {
            _board = board;
            Color = color;
        }

        public Color Color { get; init; }

        public void MoveChecker(
            (int row, int col) from,
            (int row, int col) to)
        {
            _board.MoveChecker(from, to, Color);
        }
    }
}
