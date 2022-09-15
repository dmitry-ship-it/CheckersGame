namespace CheckersGame.Common
{
    public struct GameRules
    {
        public bool IsBasicCheckerCanMoveBothWays { get; init; }

        public bool IsStrongCheckerCanMoveBothWays { get; init; }

        public bool AllowMultipleSteps { get; init; }

        public int FieldSize { get; init; }

        public int CheckersCountForSinglePlayer { get; init; }

        public static GameRules Default { get; } = new()
        {
            IsBasicCheckerCanMoveBothWays = false,
            IsStrongCheckerCanMoveBothWays = true,
            AllowMultipleSteps = true,
            FieldSize = 8,
            CheckersCountForSinglePlayer = 12,
        };

        public static GameRules International { get; } = Default with
        {
            FieldSize = 10,
            CheckersCountForSinglePlayer = 20
        };
    }
}
