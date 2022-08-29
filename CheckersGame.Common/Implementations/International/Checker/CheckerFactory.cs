using CheckersGame.Common.Abstractions.Checker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International.Checker
{
    internal class CheckerFactory
    {
        public IChecker CreateNewBlack(CheckerType checkerType)
        {
            return CreateNew(checkerType, Color.Black);
        }

        public IChecker CreateNewWhite(CheckerType checkerType)
        {
            return CreateNew(checkerType, Color.White);
        }

        private IChecker CreateNew(CheckerType checkerType, Color color)
        {
            return checkerType switch
            {
                CheckerType.Basic => new BasicChecker(color),
                CheckerType.Strong => new StrongChecker(color),
                _ => throw new ArgumentException("Invalid checker type.", nameof(checkerType)),
            };
        }
    }
}
