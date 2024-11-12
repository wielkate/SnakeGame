using System;
using System.Threading;
using Pastel;
using static SnakeGame.Colors;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;
using static SnakeGame.Sounds;

namespace SnakeGame
{
    // board[fromTop, fromLeft]
    // tuple(fromLeft, fromTop)
    // setCursorPosition(fromLeft, fromTop)
    internal class SnakeGame
    {
        internal static readonly PointType[,] board = new PointType[BOARD_HEIGHT, BOARD_WIDTH];

        private static readonly Random random = new Random();
        private static readonly Snake snake = new Snake();
        private static Direction currentDirection = Direction.Down;
        private static int score = 0;

        internal static void Play()
        {
            DrawBoard();
            snake.DrawSnake();
            DrawFruits();

            while (true)
            {
                currentDirection = ReadDirection(currentDirection);
                MoveSnake(currentDirection);
                snake.DrawSnake();
                Thread.Sleep(TIMEOUT);
            }
        }

        private static Direction ReadDirection(Direction currentDirection)
        {
            if (!Console.KeyAvailable)
            {
                return currentDirection;
            }

            var key = Console.ReadKey(true).Key;
            switch (key)
            {
                case ConsoleKey.LeftArrow when currentDirection != Direction.Right:
                    return Direction.Left;
                case ConsoleKey.RightArrow when currentDirection != Direction.Left:
                    return Direction.Right;
                case ConsoleKey.UpArrow when currentDirection != Direction.Down:
                    return Direction.Up;
                case ConsoleKey.DownArrow when currentDirection != Direction.Up:
                    return Direction.Down;
                default:
                    return currentDirection;
            };
        }

        private static void MoveSnake(Direction direction)
        {
            switch (direction)
            {
                case Direction.Left:
                    ChangePosition(-2, 0); // char size is 16 px wide (so +-2)
                    break;
                case Direction.Right:
                    ChangePosition(2, 0);
                    break;
                case Direction.Up:
                    ChangePosition(0, -1); // char size is 8 px hight (so +-1)
                    break;
                case Direction.Down:
                    ChangePosition(0, 1);
                    break;
            }
        }

        private static void ChangePosition(int changedLeft, int changedTop)
        {
            (int oldLeft, int oldTop) = snake.Head;
            (int newLeft, int newTop) = (oldLeft + changedLeft, oldTop + changedTop);

            if (newLeft < 0 || newTop < 0 || board[newTop, newLeft] == PointType.Wall)
            {
                ThrowAnException("Game is over. You just hit the wall");
            }
            else if (board[newTop, newLeft] == PointType.Snake)
            {
                ThrowAnException("Game is over. Don't eat youself!");
            }
            else if (board[newTop, newLeft] == PointType.Free)
            {
                snake.MoveTailToOldHead();
                snake.MoveHeadTo(newLeft, newTop);
            }
            else if (board[newTop, newLeft] == PointType.Fruit)
            {
                pickSound.Play();
                snake.Grow();
                snake.MoveHeadTo(newLeft, newTop);
                DrawFruit();
                score++;
            }
            else
            {
                ThrowAnException($"Error in ChangePosition() method for arguments {newLeft}, {newTop}");
            }
        }
        private static void DrawFruits()
        {
            for (var i = 0; i < FRUITS_NUMBER; i++)
            {
                DrawFruit();
            }
        }

        private static void DrawFruit()
        {
            var randomIndex = random.Next(FRUITS.Length);
            var randomFruit = FRUITS[randomIndex];

            int fromLeft;
            int fromTop;
            do
            {
                fromLeft = (random.Next(BOARD_WIDTH / 2) * 2) + 1; // odd nums
                fromTop = random.Next(BOARD_HEIGHT);
            }
            while (board[fromTop, fromLeft] != PointType.Free);

            board[fromTop, fromLeft] = PointType.Fruit;
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.Write(randomFruit.PastelBg(DARK_BROWN));
        }

        private static void DrawBoard()
        {
            for (var fromTop = 0; fromTop < BOARD_HEIGHT; fromTop++)
            {
                for (var fromLeft = 0; fromLeft < BOARD_WIDTH; fromLeft++)
                {
                    if (fromTop == 0 || fromLeft == 0 || fromTop == BOARD_HEIGHT - 1 || fromLeft == BOARD_WIDTH - 1)
                    {
                        board[fromTop, fromLeft] = PointType.Wall;
                        Console.SetCursorPosition(fromLeft, fromTop);
                        Console.Write(BOARD.PastelBg(DARK_BROWN));
                    }
                    else
                    {
                        board[fromTop, fromLeft] = PointType.Free;
                        Console.Write(" ".PastelBg(DARK_BROWN));
                    }
                }
            }
        }

        private static void ThrowAnException(string message)
        {
            pickSound.Stop();
            gameOverSound.PlaySync();
            Console.SetCursorPosition(0, BOARD_HEIGHT + 1);
            Console.WriteLine($"Your score is {score}".Pastel(BLUE));
            throw new Exception(message.Pastel(RED));
        }
    }
}