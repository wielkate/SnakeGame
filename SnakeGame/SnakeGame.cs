using System;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;

namespace SnakeGame
{
    // board[fromTop, fromLeft]
    // tuple(fromLeft, fromTop)
    // setCursorPosition(fromLeft, fromTop)
    internal class SnakeGame
    {
        internal static readonly PointType[,] board = new PointType[BOARD_HEIGHT, BOARD_WIDTH];
        internal static readonly List<(int fromLeft, int fromTop)> freePoints = new List<(int, int)>();
        internal static (int fromLeft, int fromTop) fruitPoint;

        private static readonly Snake snake = new Snake();
        private static readonly Random random = new Random();
        private static Direction currentDirection = Direction.Down;

        internal static void Play()
        {
            SetupConsole();
            DrawBoard();
            snake.DrawSnake();
            DrawFruit();

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
                    ChangePosition(-2, 0);
                    break;
                case Direction.Right:
                    ChangePosition(2, 0);
                    break;
                case Direction.Up:
                    ChangePosition(0, -1);
                    break;
                case Direction.Down:
                    ChangePosition(0, 1);
                    break;
                default:
                    break;
            }
        }

        private static void ChangePosition(int changedLeft, int changedTop)
        {
            (int oldLeft, int oldTop) = snake.Head;
            (int newLeft, int newTop) = (oldLeft + changedLeft, oldTop + changedTop);

            if (board[newTop, newLeft] == PointType.Wall)
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
                snake.Grow();
                snake.MoveHeadTo(newLeft, newTop);
                DrawFruit();
            }
            else
            {
                ThrowAnException($"Error in ChangePosition() method for arguments {newLeft}, {newTop}");
            }
        }

        internal static void DrawFruit()
        {
            var filteredFreePoints = freePoints.Where(point => point.fromLeft % 2 == 1).ToList();
            var randomIndex = random.Next(filteredFreePoints.Count);
            fruitPoint = filteredFreePoints[randomIndex];
            _ = freePoints.Remove((fruitPoint.fromLeft, fruitPoint.fromTop));
            board[fruitPoint.fromTop, fruitPoint.fromLeft] = PointType.Fruit;
            Console.SetCursorPosition(fruitPoint.fromLeft, fruitPoint.fromTop);
            Console.Write(FRUIT);
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
                        Console.Write(BOARD);
                    }
                    else
                    {
                        board[fromTop, fromLeft] = PointType.Free;
                        freePoints.Add((fromLeft, fromTop));
                    }
                }
            }
        }

        private static void SetupConsole()
        {
            Console.Title = CONSOLE_TITLE;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }

        private static void ThrowAnException(string message)
        {
            Console.SetCursorPosition(0, BOARD_HEIGHT + 1);
            Console.ForegroundColor = ConsoleColor.Red;
            throw new Exception(message);
        }
    }
}