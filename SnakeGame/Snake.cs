﻿using System;
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
            DrawSnakePartAtPoint(SNAKE_HEAD, Head.fromLeft, Head.fromTop);
            foreach ((var fromLeft, var fromTop) in Body)
            {
                DrawSnakePartAtPoint(SNAKE_BODY, fromLeft, fromTop);
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

        private void DrawSnakePartAtPoint(string snakePart, int fromLeft, int fromTop)
        {
            SnakeGame.board[fromTop, fromLeft] = PointType.Snake;
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.Write(snakePart);
        }

        private void RemoveSnakePartAtPoint(int fromLeft, int fromTop)
        {
            SnakeGame.board[fromTop, fromLeft] = PointType.Free;
            Console.SetCursorPosition(fromLeft, fromTop);
            Console.Write(" ");
        }
    }
}
