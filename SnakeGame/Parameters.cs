namespace SnakeGame
{
    internal static class Parameters
    {
        internal static readonly int BOARD_HEIGHT = 20;
        internal static readonly int BOARD_WIDTH = 50; // use numbers that are not divided by 4
        internal static readonly int TIMEOUT = 100; // the smaller timeout, the highter the speed
        internal static readonly int FRUITS_NUMBER = 5;

        internal static readonly string[] FRUITS = { "🍎", "🍊", "🍐", "🍉", "🍌", "🍒", "🥝", "🥕", "🥦", "🥑" };

        internal static readonly string CONSOLE_TITLE = "Snake game";
        internal static readonly string BOARD = "░";
        internal static readonly string SNAKE_BODY = "🟢";
        internal static readonly string SNAKE_HEAD = "👽";

        internal static readonly int CONSOLE_WIDTH = 120;
        internal static readonly int CONSOLE_HEIGHT = 30;
    }
}
