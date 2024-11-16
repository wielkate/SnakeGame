namespace SnakeGame
{
    internal static class Parameters
    {
        internal static readonly string[] FRUITS = { "🍎", "🍊", "🍐", "🍉", "🍌", "🍒", "🥝", "🥕", "🥦", "🥑" };

        internal static readonly string BOARD_DIM = "░";
        internal static readonly string BOARD_NORMAL = "▒";
        internal static readonly string BOARD_BRIGHT = "▓";

        internal static readonly string CONSOLE_TITLE = "Snake game";

        internal static readonly string SNAKE_BODY_GREEN = "🟢";
        internal static readonly string SNAKE_BODY_VIOLET = "🟣";
        internal static readonly string SNAKE_BODY_WHITE = "⚪";

        internal static readonly string SNAKE_HEAD_GREEN = "👽";
        internal static readonly string SNAKE_HEAD_VIOLET = "😈";
        internal static readonly string SNAKE_HEAD_WHITE = "💀";

        internal static readonly int SPEED_SLOW = 200;
        internal static readonly int SPEED_MEDIUM = 150;
        internal static readonly int SPEED_FAST = 100;

        internal static int BOARD_HEIGHT = 20;
        internal static int BOARD_WIDTH = 25;
        internal static int FRUITS_NUMBER = 5;
        internal static int TIMEOUT = 100; // the smaller timeout, the highter the speed

        internal static string BOARD = "░";
        internal static string SNAKE_BODY = "🟢";
        internal static string SNAKE_HEAD = "👽";
    }
}
