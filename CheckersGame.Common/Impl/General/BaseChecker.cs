using CheckersGame.Common.Abstractions.Checker;
using System.Drawing;

namespace CheckersGame.Common.Impl.General
{
    internal abstract class BaseChecker : IChecker
    {
        protected BaseChecker(Color color)
        {
            Color = color;
        }

        public abstract IMoveset Moveset { get; set; }

        public Color Color { get; init; }

        public abstract object Clone();
    }
}