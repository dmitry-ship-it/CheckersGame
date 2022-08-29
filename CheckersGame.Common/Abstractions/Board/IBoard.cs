using CheckersGame.Common.Abstractions.Checker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Abstractions.Board
{
    public interface IBoard
    {
        int Rows { get; init; }
        int Cols { get; init; }

        ICell this[int row, int col] { get; set; }

        void MoveChecker(
            (int row, int col) from,
            (int row, int col) to,
            Color playerColor);
    }
}
