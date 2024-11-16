namespace SnakeGame
{
    internal class Enum
    {
        internal enum Direction
        {
            Up,
            Down,
            Left,
            Right
        }

        internal enum PointType
        {
            Free,
            Wall,
            Fruit,
            Snake
        }

        internal enum Row
        {
            BoardHeight,
            BoardWidth,
            FruitNumber,
            Speed,
            Board,
            Snake,
            Info
        }

        internal enum Tab
        {
            Play,
            About,
            Exit,
        }
    }
}
