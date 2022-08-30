using CheckersGame.Common.Abstractions.Checker;
using System.Drawing;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal class InternationalStrongChecker : IChecker
    {
        public InternationalStrongChecker(Color color)
        {
            Color = color;
            Moveset = new InternationalStrongMoveset();
        }

        public IMoveset Moveset { get; set; }
        public Color Color { get; init; }

        public object Clone()
        {
            return new InternationalStrongChecker(Color)
            {
                Moveset = Moveset
            };
        }
    }
}