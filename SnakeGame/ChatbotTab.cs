using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spectre.Console;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;

namespace SnakeGame
{
    internal class ChatbotTab
    {
        internal static async Task TabAsync()
        {
            Console.Clear();
            var layout = CreateLayout();
            var message = new StringBuilder();

            AnsiConsole.Live(layout).Start(ctx =>
            {
                ctx.Refresh();
                while (true)
                {
                    var keyInfo = Console.ReadKey(true);

                    if (keyInfo.Key == ConsoleKey.Enter)
                    {
                        break;
                    }

                    if (keyInfo.Key == ConsoleKey.Backspace && message.Length > 0)
                    {
                        message.Remove(message.Length - 1, 1);
                    }
                    else if (keyInfo.KeyChar != '\0' && message.Length < 300)
                    {
                        message.Append(keyInfo.KeyChar);
                    }

                    layout[Row.Message.ToString()].Update(GetMessagePanel(message.ToString()));
                    ctx.Refresh();
                }
                layout[Row.Chat.ToString()].Update(GetChatPanel(true));
                ctx.Refresh();
                Thread.Sleep(2000);
            });


            string modelResponse = await AI.Call(message.ToString());
            PrepareSnakeGame(modelResponse);
        }

        private static Layout CreateLayout()
        {
            var layout = new Layout().SplitColumns(
                new Layout().SplitRows(
                    new Layout(Row.Chat.ToString()),
                    new Layout(Row.Message.ToString())
                ).Ratio(3),
                new Layout(Row.Info.ToString()).Ratio(2)
            );

            var infoPanel = GetInfoPanel();
            var chatPanel = GetChatPanel();
            var messagePanel = GetMessagePanel();

            layout[Row.Info.ToString()].Update(infoPanel);
            layout[Row.Chat.ToString()].Update(chatPanel);
            layout[Row.Message.ToString()].Update(messagePanel);

            return layout;
        }

        private static Panel GetMessagePanel(string currentMessage = "")
        {
            var headerName = "Your message";
            var user = "\n\n[bold white]User: [/]\n\n";
            var placeholderText = "[dim]Start describing your Snake game...[/][bold]▌[/]  [dim]0/300[/]";

            string body;
            if (string.IsNullOrEmpty(currentMessage))
            {
                body = user + placeholderText;
            }
            else
            {
                var escaped = Markup.Escape(currentMessage);
                body = $"{user}{escaped}[bold]▌[/]  [dim]{currentMessage.Length}/300[/]";
            }

            return new Panel(new Markup(body))
                .Header(headerName)
                .BorderColor(Color.Green4)
                .Expand()
                .RoundedBorder();
        }

        private static Panel GetChatPanel(bool withThinkingText = false)
        {
            var headerName = "Chat";
            var text = $@"

[bold white]🐍 Snake: [/][white] Hey there! What's on your mind?

[dim][italic]Example: Fast green snake on a large bright board with lots of fruits[/][/][/] 
";
            var thinkingText = $@"

[bold white]🐍 Snake: [/][white] Got you! Thinking ...[/] 
";

            if (withThinkingText)
            {
                text += thinkingText;
            }

            return new Panel(new Markup(text))
                    .Header(headerName)
                    .BorderColor(Color.LightGoldenrod3)
                    .Expand()
                    .RoundedBorder();
        }

        private static Panel GetInfoPanel()
        {
            var infoText = $@"


[mistyrose1]You're talking to the Snake Game [deeppink1_1]AI[/] — describe the world you want, and I'll build it.


Try mentioning things like:

• [deeppink1_1]Snake color[/] — Green, White, or the 
  enigmatic Violet  
• [deeppink1_1]Speed[/] — Slow, Medium, or Fast 
  (I won't judge your reflexes)  
• [deeppink1_1]Board style[/] — Small, Huge, Square, Dim, 
  Bright, or just Normal  
• [deeppink1_1]Fruit amount[/] — Few, Many, or A Lot 
  (for maximum snacking)

Skip anything you like; I'll fill in the rest with smart AI defaults.

Press [invert] ENTER [/] to command your digital serpent [/]
";

            return new Panel(new Markup(infoText))
                .BorderColor(Color.LightGoldenrod3)
                .Expand()
                .RoundedBorder();
        }

        private static void PrepareSnakeGame(string response)
        {
            string cleaned = response
                .Replace("```json", "")
                .Replace("```", "")
                .Trim();

            GameSettings settings = JsonConvert.DeserializeObject<GameSettings>(cleaned);

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

            TIMEOUT = chooseSpeed[settings.Speed];
            BOARD = chooseBoard[settings.BoardColor];
            SNAKE_HEAD = chooseSnakeHead[settings.SnakeColor];
            SNAKE_BODY = chooseSnakeBody[settings.SnakeColor];

            FRUITS_NUMBER = settings.FruitsNumber;
            BOARD_HEIGHT = settings.BoardHeight;
            BOARD_WIDTH = settings.BoardWidth * 2; // use even numbers

            Console.CursorVisible = false;
            AnsiConsole.Clear();
        }
    }
}

