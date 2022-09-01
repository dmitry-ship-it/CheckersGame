namespace CheckersGame.Api.Core
{
    public class PlayerInfo
    {
        public Guid Id => Guid.NewGuid();

        public string Name { get; set; }

        public PlayerInfo(string? name)
        {
            Name = name ?? string.Empty;
        }
    }
}
