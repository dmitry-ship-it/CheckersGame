namespace CheckersGame.Common.Abstractions
{
    public interface IMover
    {
        void MoveChecker(
            (int row, int col) from,
            (int row, int col) to);
    }
}