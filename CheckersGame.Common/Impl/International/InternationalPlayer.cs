using CheckersGame.Common.Impl.General;
using System.Drawing;

namespace CheckersGame.Common.Impl.International
{
    internal class InternationalPlayer : BasePlayer
    {
        public InternationalPlayer(Color color)
            : base(20)
        {
            Color = color;
        }

        public override Color Color { get; init; }
    }
}