using System;
using System.Collections.Generic;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;

namespace SnakeGame
{

    internal class Snake
    {
        internal (int fromLeft, int fromTop) Head { get; set; }
        private readonly Queue<(int, int)> Body;
        internal Snake()
        {
            Head = (BOARD_WIDTH / 2, BOARD_HEIGHT / 2);
            Body = new Queue<(int, int)>();
            Body.Enqueue((BOARD_WIDTH / 2, (BOARD_HEIGHT / 2) - 1));
        }
        internal void DrawSnake()
        {
            DrawSnakeAtPoint(SNAKE_HEAD, Head.fromLeft, Head.fromTop);
            foreach ((var fromLeft, var fromTop) in Body)
            {
                DrawSnakeAtPoint(SNAKE_BODY, fromLeft, fromTop);
            }
        }

        internal void MoveTailToOldHead()
        {
            (var tailLeft, var tailTop) = Body.Dequeue();
            RemoveSnakePartAtPoint(tailLeft, tailTop);
            Body.Enqueue((Head.fromLeft, Head.fromTop));
        }

        internal void MoveHeadTo(int newFromLeft, int newFromTop)
        {
            Head = (newFromLeft, newFromTop);
        }

        internal void Grow()
        {
            Body.Enqueue((Head.fromLeft, Head.fromTop));
        }

        private void DrawSnakeAtPoint(string snakePart, int fromLeft, int fromTop)
        {
            SnakeGame.board[fromTop, fromLeft] = PointType.Snake;
            _ = SnakeGame.freePoints.Remove((fromLeft, fromTop));
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.Write(snakePart);

            Console.SetCursorPosition(0, BOARD_HEIGHT + 2);
            Console.WriteLine($"Snake position {(fromLeft, fromTop)}");
        }

        private void RemoveSnakePartAtPoint(int fromLeft, int fromTop)
        {
            SnakeGame.board[fromTop, fromLeft] = PointType.Free;
            SnakeGame.freePoints.Add((fromLeft, fromTop));
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.Write(" ");
        }
    }
}
