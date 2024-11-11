using System.Media;

namespace SnakeGame
{
    internal static class Sounds
    {
        internal static readonly SoundPlayer gameOverSound = new SoundPlayer("Sounds/game_over.wav");
        internal static readonly SoundPlayer introSound = new SoundPlayer("Sounds/intro.wav");
        internal static readonly SoundPlayer pickSound = new SoundPlayer("Sounds/pick.wav");
        internal static readonly SoundPlayer selectSound = new SoundPlayer("Sounds/select.wav");
    }
}
