using CheckersGame.Common.Abstractions.Board;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Abstractions.Checker
{
    public interface IMoveset
    {
        bool IsLegalMove(
            (int row, int col) from,
            (int row, int col) to,
            IBoard board);
        (int row, int col)? TryGetEnemyPosOnTheWay((int row, int col) from, (int row, int col) to, IBoard board);
    }
}
