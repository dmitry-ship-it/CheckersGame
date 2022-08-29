using CheckersGame.Common.Abstractions.Checker;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CheckersGame.Common.Implementations.International.Checker
{
    internal class StrongChecker : IChecker
    {
        public StrongChecker(Color color)
        {
            Moveset = new StrongMoveset();
            Color = color;
        }

        public IMoveset Moveset { get; init; }

        public Color Color { get; init; }

        public override string ToString() => "Strong";
    }
}
