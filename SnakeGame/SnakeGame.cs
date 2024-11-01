using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using static SnakeGame.Enum;

namespace SnakeGame
{
    // board[fromTop, fromLeft]
    // tuple(fromLeft, fromTop)
    // setCursorPosition(fromLeft, fromTop)
    internal class SnakeGame
    {
        private static readonly List<(int, int)> snakeBody = new List<(int fromLeft, int fromTop)>();
        private static readonly List<(int, int)> freePoints = new List<(int fromLeft, int fromTop)>();
        private static readonly PointType[,] board = new PointType[Properties.BOARD_HEIGHT, Properties.BOARD_WIDTH];
        private static readonly Random random = new Random();

        private static Direction currentDirection = Direction.Down;

        internal static void Play()
        {
            SetupConsole();
            DrawBoard();

            // Set up snake in the middle
            Console.SetCursorPosition(Properties.BOARD_WIDTH / 2, Properties.BOARD_HEIGHT / 2);
            board[Properties.BOARD_HEIGHT / 2, Properties.BOARD_WIDTH / 2] = PointType.Snake;
            snakeBody.Add((Properties.BOARD_WIDTH / 2, Properties.BOARD_HEIGHT / 2));
            freePoints.Remove((Properties.BOARD_WIDTH / 2, Properties.BOARD_HEIGHT / 2));

            DrawSnake();
            DrawFruit();

            while (true)
            {
                Console.SetCursorPosition(0, Properties.BOARD_HEIGHT + 1);
                currentDirection = ReadDirection(currentDirection);
                MoveSnake(currentDirection);
                DrawSnake();
                Thread.Sleep(Properties.TIMEOUT);
            }
        }

        private static void DrawFruit()
        {
            var randomIndex = random.Next(freePoints.Count);
            (int fromLeft, int fromTop) = freePoints[randomIndex];
            board[fromTop, fromLeft] = PointType.Fruit;
            freePoints.Remove((fromLeft, fromTop));
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.WriteLine(Properties.FRUIT);
        }

        private static Direction ReadDirection(Direction currentDirection)
        {
            if (!Console.KeyAvailable) return currentDirection;
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
            (int fromLeft, int fromTop) = snakeBody.First();
            switch (direction)
            {
                case Direction.Left:
                    ChangePosition(fromLeft - 1, fromTop);
                    break;
                case Direction.Right:
                    ChangePosition(fromLeft + 1, fromTop);
                    break;
                case Direction.Up:
                    ChangePosition(fromLeft, fromTop - 1);
                    break;
                case Direction.Down:
                    ChangePosition(fromLeft, fromTop + 1);
                    break;
            }
        }

        private static void ChangePosition(int newFromLeft, int newFromTop)
        {
            if (board[newFromTop, newFromLeft] == PointType.Wall) throw new Exception("Game is over. You just hit the wall");
            else if (board[newFromTop, newFromLeft] == PointType.Snake) throw new Exception("Game is over. Don't eat youself!");
            
            else if (board[newFromTop, newFromLeft] == PointType.Free)
            {
                board[newFromTop, newFromLeft] = PointType.Snake;
                RemoveSnakeTail();
                snakeBody.Add((newFromLeft, newFromTop));
                freePoints.Remove((newFromLeft, newFromTop));
            }

            else if (board[newFromTop, newFromLeft] == PointType.Fruit)
            {
                board[newFromTop, newFromLeft] = PointType.Snake;
                snakeBody.Add((newFromLeft, newFromTop));
                freePoints.Remove((newFromLeft, newFromTop));
                DrawFruit();
            }

            else throw new Exception($"Error in ChangePosition() method for arguments {newFromLeft}, {newFromTop}");          
        }

        private static void DrawSnake()
        {
            foreach ((int fromLeft, int fromTop) in snakeBody)
            {
                Console.SetCursorPosition(fromLeft, fromTop);
                Console.WriteLine(Properties.SNAKE_BODY);
            }
        }

        private static void RemoveSnakeTail()
        {
            (int fromLeft, int fromTop) = snakeBody.Last();
            board[fromTop, fromLeft] = PointType.Free;
            snakeBody.Remove((fromLeft, fromTop));
            freePoints.Add((fromLeft, fromTop));
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.WriteLine(" ");
        }

        private static void DrawBoard()
        {
            for (var fromTop = 0; fromTop < Properties.BOARD_HEIGHT; fromTop++)
            {
                for (var fromLeft = 0; fromLeft < Properties.BOARD_WIDTH; fromLeft++)
                {
                    if (fromTop == 0 || fromLeft == 0 || fromTop == Properties.BOARD_HEIGHT - 1)
                    {
                        board[fromTop, fromLeft] = PointType.Wall;
                        Console.SetCursorPosition(fromLeft, fromTop);
                        Console.Write(Properties.BOARD);

                    }
                    else if (fromLeft == Properties.BOARD_WIDTH - 1)
                    {
                        board[fromTop, fromLeft] = PointType.Wall;
                        Console.SetCursorPosition(fromLeft + 1, fromTop);
                        Console.Write(Properties.BOARD);
                        Console.SetCursorPosition(fromLeft + 1, fromTop + 1);
                        Console.Write(Properties.BOARD);
                        Console.SetCursorPosition(fromLeft + 1, fromTop - 1);
                        Console.Write(Properties.BOARD);

                    }
                    else
                    {
                        board[fromTop, fromLeft] = PointType.Free;
                        freePoints.Add((fromLeft,  fromTop));
                    }
                }
            }
        }

        private static void SetupConsole()
        {
            Console.Title = Properties.CONSOLE_TITLE;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }
    }
}
