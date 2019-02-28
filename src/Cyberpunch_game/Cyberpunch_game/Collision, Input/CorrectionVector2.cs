namespace Cyberpunch_game
{
    struct CorrectionVector2
    {
        public DirectionX DirectionX;
        public DirectionY DirectionY;
        public float X;
        public float Y;
    }

    public enum DirectionX
    {
        Left = -1,
        None = 0,
        Right = 1
    }

    public enum DirectionY
    {
        Up = -1,
        None = 0,
        Down = 1
    }
}
