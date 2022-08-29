namespace CheckersGame.Api.Models
{
    public class MoveModel
    {
        public Pair From { get; set; }
        public Pair To { get; set; }
    }

    public struct Pair
    {
        public int Row { get; set; }
        public int Col { get; set; }
    }
}
