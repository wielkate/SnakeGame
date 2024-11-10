using System;
using System.Drawing;
using System.Text;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;

namespace SnakeGame
{
    internal class Menu
    {
        private static readonly Tab[] tabs = { Tab.Play, Tab.About, Tab.Exit };
        private static readonly string asciiChars = " .:-=+*#%@";
        private static readonly Bitmap image = new Bitmap("snake.png");

        private static int selectedIndex = 0;
        private static int menuTopPosition;

        internal static void Display()
        {
            SetupConsole();
            MainTab();
        }

        private static void MainTab()
        {
            Console.Clear();
            DisplayImage();
            DisplayWelcomeText();
            DisplayMenuOptions();

            var key = Console.ReadKey(true).Key;
            while (key != ConsoleKey.Enter)
            {
                selectedIndex = SelectIndex(key);
                DisplayMenuOptions();
                key = Console.ReadKey(true).Key;
            }

            DisplayTab(tabs[selectedIndex]);
        }

        private static void DisplayTab(Tab tab)
        {
            switch (tab)
            {
                case Tab.Play:
                    PlayTab();
                    break;
                case Tab.About:
                    AboutTab();
                    break;
                case Tab.Exit:
                    break;
            }
        }

        private static void PlayTab()
        {
            Console.Clear();
            SnakeGame.Play();
        }

        private static void AboutTab()
        {
            Console.Clear();
            var title = @"                                                          
         .8.          8 888888888o       ,o888888o.     8 8888      88 8888888 8888888888 
        .888.         8 8888    `88.  . 8888     `88.   8 8888      88       8 8888       
       :88888.        8 8888     `88 ,8 8888       `8b  8 8888      88       8 8888       
      . `88888.       8 8888     ,88 88 8888        `8b 8 8888      88       8 8888       
     .8. `88888.      8 8888.   ,88' 88 8888         88 8 8888      88       8 8888       
    .8`8. `88888.     8 8888888888   88 8888         88 8 8888      88       8 8888       
   .8' `8. `88888.    8 8888    `88. 88 8888        ,8P 8 8888      88       8 8888       
  .8'   `8. `88888.   8 8888      88 `8 8888       ,8P  ` 8888     ,8P       8 8888       
 .888888888. `88888.  8 8888    ,88'  ` 8888     ,88'     8888   ,d8P        8 8888       
.8'       `8. `88888. 8 888888888P       `8888888P'        `Y88888P'         8 8888                                 

";
            var text = @"
Game Rules
Get ready to slither, eat, and grow! 
Guide your snake across the board using arrows, gobbling up treats to grow longer and score higher. 
But watch out — running into walls or your own tail will end the game!

I hope you enjoy playing the game as much as I enjoyed developing it! Happy snake guiding! 
";
            Console.WriteLine(title);
            Console.WriteLine(@"This game was developed as part of an academic project for the User Interface (UI) subject.

Resources:");
            Console.Write("1. ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("https://patorjk.com/software/taag/ ");
            Console.ResetColor();
            Console.WriteLine("(assets)");
            Console.Write("2. ");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.Write("https://paulbourke.net/dataformats/asciiart/ ");
            Console.ResetColor();
            Console.WriteLine("(ASCII Characters)");
            Console.WriteLine(text);
            Console.ForegroundColor = ConsoleColor.DarkGray;

            Console.WriteLine("[Press any key to return back to the menu.]");
            Console.ResetColor();
            _ = Console.ReadKey(true);
            MainTab();
        }

        private static int SelectIndex(ConsoleKey key)
        {
            switch (key)
            {
                case ConsoleKey.UpArrow when selectedIndex != 0:
                    return --selectedIndex;
                case ConsoleKey.DownArrow when selectedIndex != tabs.Length - 1:
                    return ++selectedIndex;
                default:
                    return selectedIndex;
            };
        }

        private static void DisplayMenuOptions()
        {
            Console.SetCursorPosition(0, menuTopPosition);
            for (int i = 0; i < tabs.Length; i++)
            {
                var option = tabs[i];
                if (i == selectedIndex)
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    if (option == Tab.Exit)
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                    }
                }
                Console.WriteLine($"<< {option} >>");
                Console.ResetColor();
            }
        }

        private static void DisplayWelcomeText()
        {
            var title = "🐍 Welcome to Classic Snake Game! 🐍\n";
            var text = "[Use the up and down keys for menu navigation. Press enter to choose the option.]";

            Console.SetCursorPosition((Console.WindowWidth - title.Length) / 2, menuTopPosition++);
            Console.WriteLine(title);
            Console.ForegroundColor = ConsoleColor.DarkGray;
            Console.WriteLine(text);
            Console.ResetColor();
            menuTopPosition += 3;
        }

        private static void DisplayImage()
        {
            var ratio = 2 * image.Width / image.Height; // char size is 16 px wide x 8 px hight
            var width = Console.WindowWidth - 33;
            var height = width / ratio;

            using (var img = new Bitmap(image, new Size(width, height)))
            {
                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        var pixelColor = img.GetPixel(j, i);
                        var greyValue = (int)((pixelColor.G + pixelColor.B + pixelColor.R) / 3.0);
                        var asciiIndex = greyValue * (asciiChars.Length - 1) / 255;
                        Console.Write(asciiChars[asciiIndex]);
                    }
                    Console.WriteLine();
                }
                menuTopPosition = ++height;
            }
        }

        private static void SetupConsole()
        {
            Console.Title = CONSOLE_TITLE;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }
    }
}
