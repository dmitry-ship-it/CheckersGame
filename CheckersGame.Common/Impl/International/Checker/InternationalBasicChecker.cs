using CheckersGame.Common.Abstractions.Checker;
using CheckersGame.Common.Impl.General;
using System.Drawing;

namespace CheckersGame.Common.Impl.International.Checker
{
    internal class InternationalBasicChecker : BaseChecker
    {
        public InternationalBasicChecker(Color color) : base(color)
        {
            Moveset = new InternationalBasicMoveset();
        }

        public override IMoveset Moveset { get; set; }

        public static IChecker Empty => null!;

        public override object Clone()
        {
            return new InternationalBasicChecker(Color)
            {
                Moveset = Moveset
            };
        }
    }
}