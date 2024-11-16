using System;
using System.Collections.Generic;
using System.Linq;
using Spectre.Console;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;
using static SnakeGame.Sounds;

namespace SnakeGame
{
    internal class CustomTab
    {
        private static readonly Row[] rows = { Row.BoardHeight, Row.BoardWidth, Row.FruitNumber, Row.Speed, Row.Board, Row.Snake };
        private static readonly string[] speed = { "Slow", "Medium", "Fast" };
        private static readonly string[] board = { "Dim", "Normal", "Bright" };
        private static readonly string[] snakeColor = { "White", "Green", "Violet" };
        private static int selectedRowIndex = 0;
        private static int speedIndex = 2;
        private static int boardIndex = 0;
        private static int snakeColorIndex = 1;

        internal static void Tab()
        {
            Console.Clear();
            var layout = CreateLayout();
            AnsiConsole.Write(layout);
            Console.SetCursorPosition(0, 0);

            var key = Console.ReadKey(true).Key;
            AnsiConsole.Live(layout).Start(ctx =>
            {
                while (key != ConsoleKey.Enter)
                {
                    if (key == ConsoleKey.R)
                    {
                        DiscardChanges(layout);
                    }

                    pickSound.PlaySync();
                    selectedRowIndex = SelectRow(key);
                    var selectedRow = rows[selectedRowIndex];
                    var updatedPanel = GetUpdatedPanel(selectedRow);

                    layout[selectedRow.ToString()].Update(updatedPanel);
                    ctx.Refresh();

                    key = Console.ReadKey(true).Key;
                    updatedPanel.BorderColor(Color.LightGoldenrod3);
                };
            });

            PrepareSnakeGame();
        }

        private static void DiscardChanges(Layout layout)
        {
            speedIndex = 2;
            boardIndex = 0;
            snakeColorIndex = 1;
            BOARD_HEIGHT = 20;
            BOARD_WIDTH = 25;
            FRUITS_NUMBER = 5;
            foreach (var row in rows)
            {
                var updatedPanel = GetUpdatedPanel(row).BorderColor(Color.LightGoldenrod3);
                layout[row.ToString()].Update(updatedPanel);
            }
        }

        private static void PrepareSnakeGame()
        {
            var chooseSpeed = new Dictionary<string, int>(){
                {"Slow", SPEED_SLOW},
                {"Medium", SPEED_MEDIUM},
                {"Fast", SPEED_FAST} };
            var chooseBoard = new Dictionary<string, string>(){
                {"Dim", BOARD_DIM},
                {"Normal", BOARD_NORMAL},
                {"Bright", BOARD_BRIGHT} };
            var chooseSnakeHead = new Dictionary<string, string>(){
                {"White", SNAKE_HEAD_WHITE},
                {"Green", SNAKE_HEAD_GREEN},
                {"Violet", SNAKE_HEAD_VIOLET} };
            var chooseSnakeBody = new Dictionary<string, string>(){
                {"White", SNAKE_BODY_WHITE},
                {"Green", SNAKE_BODY_GREEN},
                {"Violet", SNAKE_BODY_VIOLET} };

            TIMEOUT = chooseSpeed[speed[speedIndex]];
            BOARD = chooseBoard[board[boardIndex]];
            SNAKE_HEAD = chooseSnakeHead[snakeColor[snakeColorIndex]];
            SNAKE_BODY = chooseSnakeBody[snakeColor[snakeColorIndex]];
            BOARD_WIDTH *= 2; // use even numbers

            Console.CursorVisible = false;
            selectSound.PlaySync();
            AnsiConsole.Clear();
        }

        private static Panel GetUpdatedPanel(Row layoutName)
        {
            switch (layoutName)
            {
                case Row.BoardHeight:
                    return GetBoarderHeightPanel().BorderColor(Color.Green4);
                case Row.BoardWidth:
                    return GetBoardWidthPanel().BorderColor(Color.Green4);
                case Row.FruitNumber:
                    return GetFruitNumberPanel().BorderColor(Color.Green4);
                case Row.Speed:
                    return GetSpeedPanel(speedIndex).BorderColor(Color.Green4);
                case Row.Board:
                    return GetBoardPanel(boardIndex).BorderColor(Color.Green4);
                case Row.Snake:
                    return GetSnakeColorPanel(snakeColorIndex).BorderColor(Color.Green4);
            };
            return null;
        }

        private static int SelectRow(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow when selectedRowIndex != 0:
                    return --selectedRowIndex;
                case ConsoleKey.DownArrow when selectedRowIndex != rows.Length - 1:
                    return ++selectedRowIndex;
                case ConsoleKey.LeftArrow:
                    ChangeValue(-1);
                    return selectedRowIndex;
                case ConsoleKey.RightArrow:
                    ChangeValue(1);
                    return selectedRowIndex;
            };
            return selectedRowIndex;
        }

        private static void ChangeValue(int value)
        {
            switch (rows[selectedRowIndex])
            {
                case Row.BoardHeight:
                    var newHeight = BOARD_HEIGHT + value;
                    if (newHeight >= 15 && newHeight <= 28)
                    {
                        BOARD_HEIGHT = newHeight;
                    }
                    break;
                case Row.BoardWidth:
                    var newWidth = BOARD_WIDTH + value;
                    if (newWidth >= 15 && newWidth <= 26)
                    {
                        BOARD_WIDTH = newWidth;
                    }
                    break;
                case Row.FruitNumber:
                    var newFruitNumber = FRUITS_NUMBER + value;
                    if (newFruitNumber >= 1 && newFruitNumber <= 9)
                    {
                        FRUITS_NUMBER = newFruitNumber;
                    }
                    break;
                case Row.Speed:
                    var newSpeedIndex = speedIndex + value;
                    if (newSpeedIndex >= 0 && newSpeedIndex <= speed.Length - 1)
                    {
                        speedIndex = newSpeedIndex;
                    }
                    break;

                case Row.Board:
                    var newBoardIndex = boardIndex + value;
                    if (newBoardIndex >= 0 && newBoardIndex <= board.Length - 1)
                    {
                        boardIndex = newBoardIndex;
                    }
                    break;
                case Row.Snake:
                    var newSnakeColorIndex = snakeColorIndex + value;
                    if (newSnakeColorIndex >= 0 && newSnakeColorIndex <= snakeColor.Length - 1)
                    {
                        snakeColorIndex = newSnakeColorIndex;
                    }
                    break;
            };
        }

        private static Layout CreateLayout()
        {
            var layout = new Layout().SplitColumns(
                new Layout().SplitRows(
                    new Layout(Row.BoardHeight.ToString()),
                    new Layout(Row.BoardWidth.ToString()),
                    new Layout(Row.FruitNumber.ToString()),
                    new Layout(Row.Speed.ToString()),
                    new Layout(Row.Board.ToString()),
                    new Layout(Row.Snake.ToString())
                    ),
                new Layout(Row.Info.ToString())
                );

            var boardDim = string.Concat(Enumerable.Repeat(BOARD_DIM, 5));
            var boardNormal = string.Concat(Enumerable.Repeat(BOARD_NORMAL, 5));
            var boardBright = string.Concat(Enumerable.Repeat(BOARD_BRIGHT, 5));
            var table = new Table()
                    .AddColumns("", "[bold mistyrose1]Snake[/]", "", "[bold mistyrose1]Board[/]")
                    .AddEmptyRow()
                    .AddRow("[mediumpurple2]Violet[/]", $"{SNAKE_HEAD_VIOLET}{SNAKE_BODY_VIOLET}{SNAKE_BODY_VIOLET}", "[mistyrose1]Bright[/]", $"{boardBright}")
                    .AddRow("[green4]Green[/]", $"{SNAKE_HEAD_GREEN}{SNAKE_BODY_GREEN}{SNAKE_BODY_GREEN}", "[mistyrose1]Normal[/]", $"{boardNormal}")
                    .AddRow("[white]White[/]", $"{SNAKE_HEAD_WHITE}{SNAKE_BODY_WHITE}{SNAKE_BODY_WHITE}", "[mistyrose1]Dim[/]", $"{boardDim}")
                    .AddEmptyRow()
                    .Expand()
                    .NoBorder();

            var infoText = $@"
[mistyrose1]Here’s a quick guide to help you customize your game for the best experience:

Use the [invert] UP [/] and [invert] DOWN [/] arrows to navigate through the settings.
Use the [invert] LEFT [/] and [invert] RIGHT [/] arrows to adjust each parameter.
Press [invert] R [/] to discard changes.
Press [invert] ENTER [/] to confirm your selections.

Adjust speed to match your skill level.


[deeppink1_1]Note:[/] Numerical settings have [underline]limits[/] to maintain optimal gameplay[/].
";

            var infoPanel = GetPanel(new Markup(infoText), table);
            var boarderHeightPanel = GetBoarderHeightPanel();
            var boarderWidthPanel = GetBoardWidthPanel();
            var fruitNumberPanel = GetFruitNumberPanel();
            var speedPanel = GetSpeedPanel(speedIndex);
            var boarderPanel = GetBoardPanel(boardIndex);
            var snakePanel = GetSnakeColorPanel(snakeColorIndex);

            layout[Row.Info.ToString()].Update(infoPanel);
            layout[Row.BoardHeight.ToString()].Update(boarderHeightPanel);
            layout[Row.BoardWidth.ToString()].Update(boarderWidthPanel);
            layout[Row.FruitNumber.ToString()].Update(fruitNumberPanel);
            layout[Row.Speed.ToString()].Update(speedPanel);
            layout[Row.Board.ToString()].Update(boarderPanel);
            layout[Row.Snake.ToString()].Update(snakePanel);

            return layout;
        }

        private static Panel GetSnakeColorPanel(int selectedIndex)
        {
            return GetPanel("Snake color", getColoredText(selectedIndex, snakeColor));
        }

        private static Panel GetBoardPanel(int selectedIndex)
        {
            return GetPanel("Board", getColoredText(selectedIndex, board));
        }

        private static Panel GetSpeedPanel(int selectedIndex)
        {
            return GetPanel("Speed", getColoredText(selectedIndex, speed));
        }

        private static string getColoredText(int selectedIndex, string[] array)
        {
            var text = "\n";
            for (int i = 0; i < array.Length; i++)
            {
                var textColor = (i == selectedIndex) ? Color.DarkOrange3.ToMarkup() : "";
                text += $"[{textColor}]{array[i]}[/]       ";
            }

            return text.TrimEnd();
        }

        private static Panel GetFruitNumberPanel()
        {
            return GetPanel("Fruit number", $"\n- {FRUITS_NUMBER,2} +");
        }

        private static Panel GetBoardWidthPanel()
        {
            return GetPanel("Board width", $"\n- {BOARD_WIDTH} +");
        }

        private static Panel GetBoarderHeightPanel()
        {
            return GetPanel("Board height", $"\n- {BOARD_HEIGHT} +");
        }

        private static Panel GetPanel(string name = "", string text = "")
        {
            return new Panel(new Markup(text).Centered())
                .Header(name)
                .BorderColor(Color.LightGoldenrod3)
                .Expand()
                .RoundedBorder();
        }

        private static Panel GetPanel(Markup first, Table table)
        {
            return new Panel(new Rows(first, table))
                .BorderColor(Color.LightGoldenrod3)
                .Expand()
                .Padding(2, 2, 2, 2)
                .RoundedBorder();
        }
    }
}
