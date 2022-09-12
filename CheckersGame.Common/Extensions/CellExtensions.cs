using CheckersGame.Common.Abstractions;

namespace CheckersGame.Common.Extensions
{
    public static class CellExtensions
    {
        public static Cell ToCell(this IEnumerable<int> seq)
        {
            if (seq.Count() != 2)
            {
                throw new ArgumentOutOfRangeException(nameof(seq), "Sequence contains more or less than 2 elemenets.");
            }

            return new Cell { Col = seq.First(), Row = seq.Last() };
        }

        public static Cell ToCell(this Tuple<int, int> tuple)
        {
            return new Cell { Col = tuple.Item1, Row = tuple.Item2 };
        }
    }
}
