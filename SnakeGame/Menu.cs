using System;
using System.Drawing;
using System.Text;
using System.Threading;
using Figgle;
using Pastel;

using static SnakeGame.Enum;
using static SnakeGame.Parameters;
using static SnakeGame.Colors;

namespace SnakeGame
{
    internal class Menu
    {
        private static readonly Tab[] tabs = { Tab.Play, Tab.About, Tab.Exit };
        private static readonly string asciiChars = "$@B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
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
            do
            {
                DisplayWelcomeText(); // Blinking till you press the key
            }
            while (!Console.KeyAvailable);
            menuTopPosition += 3;
            ClearConsoleAtLine(menuTopPosition);

            ConsoleKey key;
            do
            {
                key = Console.ReadKey(true).Key;
                selectedIndex = SelectIndex(key);
                DisplayMenuOptions();
            }
            while (key != ConsoleKey.Enter);

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
            var offset = new string(' ', BOARD_WIDTH);
            var title = $@"                                                                                                                                                                                                                                                                                      
{offset}                      __    __    __    __
{offset}                     /  \  /  \  /  \  /  \
{offset}____________________/  __\/  __\/  __\/  __\_________________________
{offset}___________________/  /__/  /__/  /__/  /____________________________
{offset}                  | / \   / \   / \   / \  \____
{offset}                  |/   \_/   \_/   \_/   \    o \
{offset}                                          \_____/--<";
            var text = $@"
 {offset} Get ready to slither, eat, and grow! 
 {offset} Guide your snake across the board using arrows, gobbling up treats 
 {offset} to grow longer and score higher. 
 {offset} But watch out — running into walls or your own tail will end 
 {offset} the game!

 {offset} Hope you enjoy playing the game as much as I enjoyed developing it!
 {offset} Happy snake guiding! 
";

            Thread.Sleep(500);
            Console.Clear();
            Console.WriteLine(title.Pastel(GREEN));
            Console.WriteLine(text.Pastel(BROWN));
            SnakeGame.Play();
        }

        private static void AboutTab()
        {
            var title = FiggleFonts.Broadway.Render("ABOUT");
            var text = $@"
This application was developed as part of an academic project for the User Interface (UI) subject.

Resources:

1. {"https://paulbourke.net/dataformats/asciiart/".Pastel(BLUE)} (ASCII Characters)
2. {"https://ascii.co.uk/art/snake".Pastel(BLUE)} (Jennifer E. Swofford)
3. {"https://www.pngegg.com/en/png-ogczk".Pastel(BLUE)} (Snake image)

{"[Press any key to return back to the menu.]".Pastel(DARK_BLUE)}";

            Console.Clear();
            Console.WriteLine(title);
            Console.WriteLine(text);
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
                var color = WHITE;
                var option = tabs[i];
                if (i == selectedIndex)
                {
                    color = GREEN;
                    if (option == Tab.Exit)
                    {
                        color = RED;
                    }
                }
                Console.WriteLine($"<< {option} >>".Pastel(color));
            }
        }

        private static void DisplayWelcomeText()
        {
            var title = FiggleFonts.CyberSmall.Render("                Welcome");
            var text = "[Use the up and down keys for menu navigation. Press enter to choose the option.]";
            var delay = 100;
            string[] greyShadows = { BLACK, DARK_GREY, GREY, WHITE };

            foreach (var color in greyShadows)
            {
                Console.SetCursorPosition(0, menuTopPosition);
                Console.WriteLine(title.Pastel(color));
                Console.WriteLine(text.Pastel(DARK_BROWN));
                Thread.Sleep(delay);
            }
        }

        private static void DisplayImage()
        {
            var ratio = 2 * image.Width / image.Height; // char size is 16 px wide x 8 px hight
            var width = Console.WindowWidth - 33;
            var height = width / ratio;

            using (var resizedImage = new Bitmap(image, new Size(width, height)))
            {
                for (var i = 0; i < height; i++)
                {
                    for (var j = 0; j < width; j++)
                    {
                        var pixelColor = resizedImage.GetPixel(j, i);
                        var greyValue = (int)((pixelColor.G + pixelColor.B + pixelColor.R) / 3.0);
                        var asciiIndex = greyValue * (asciiChars.Length - 1) / 255;
                        var asciiValue = char.ToString(asciiChars[asciiIndex]);
                        Console.Write(asciiValue.Pastel(Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B)));
                    }
                    Console.WriteLine();
                }
                menuTopPosition = ++height;
            }
        }

        private static void ClearConsoleAtLine(int line)
        {
            Console.SetCursorPosition(0, line);
            Console.WriteLine(new string(' ', Console.WindowWidth));
        }

        private static void SetupConsole()
        {
            Console.Title = CONSOLE_TITLE;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }
    }
}
