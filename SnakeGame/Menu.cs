using System;
using System.Drawing;
using System.Text;
using System.Threading;
using Figgle;
using Pastel;
using static SnakeGame.Colors;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;
using static SnakeGame.Sounds;

namespace SnakeGame
{
    internal class Menu
    {
        private static readonly Tab[] tabs = { Tab.Play, Tab.About, Tab.Exit };
        private static readonly string asciiChars = " @B%8&WM#*oahkbdpqwmZO0QLCJUYXzcvunxrjft/\\|()1{}[]?-_+~<>i!lI;:,\"^`'. ";
        private static readonly Bitmap image = new Bitmap("snake.png");

        private static int selectedIndex = 0;
        private static int menuTopPosition;

        internal static void Display()
        {
            SetupConsole();
            LoadSounds();
            introSound.Play();
            MainTab();
        }

        private static void MainTab()
        {
            ClearConsole(BACKGROUND_GREEN);
            DisplayImage();

            do
            {
                DisplayWelcomeText(); // Blinking till you press the key
            }
            while (!Console.KeyAvailable);
            menuTopPosition += 3;
            ClearConsoleAtLine(menuTopPosition);
            introSound.Stop();

            var key = Console.ReadKey(true).Key;
            while (key != ConsoleKey.Enter)
            {
                pickSound.PlaySync();
                selectedIndex = SelectIndex(key);
                DisplayMenuOptions();
                key = Console.ReadKey(true).Key;
            }

            DisplayTab(tabs[selectedIndex]);
        }

        private static void DisplayTab(Tab tab)
        {
            selectSound.PlaySync();
            if (tab == Tab.Play)
            {
                LoadingTab();
                PlayTab();
            }
            else if (tab == Tab.About)
            {
                AboutTab();
            }
        }

        private static void PlayTab()
        {
            var offset = new string(' ', BOARD_WIDTH);
            var title = $@"                                                                                                                                                                                                                                                                                      
{offset}                      __    __    __    __
{offset}                     /  \  /  \  /  \  /  \
{offset}____________________/  __\/  __\/  __\/  __\________________________
{offset}___________________/  /__/  /__/  /__/  /___________________________
{offset}                   | / \   / \   / \   / \  \____
{offset}                   |/   \_/   \_/   \_/   \    o \
{offset}                                           \_____/--<";
            var text = $@"
{offset} Get ready to slither, eat, and grow! 
{offset} Guide your snake across the board using arrows, gobbling up treats 
{offset} to grow longer and score higher. 
{offset} But watch out — running into walls or your own tail will end 
{offset} the game!

{offset} Hope you enjoy playing the game as much as I enjoyed developing it!
{offset} Happy snake guiding!

{offset} {"Your score is 0".Pastel(BLUE)}
";
            Console.Clear();
            Console.WriteLine(title.Pastel(GREEN));
            Console.WriteLine(text.Pastel(BROWN));
            SnakeGame.Play();
        }

        private static void LoadingTab()
        {
            countdownSound.Play();
            var delay = 40;
            var barLength = 100;
            var offset = new string(' ', (Console.WindowWidth - barLength) / 2);
            var text = $@"
                                                        Loading...


            Do you know that the very first Snake-type game was an arcade game called Blockade. 
            It was created by Gremlin way back in {"1976".Pastel(GREEN)}.
";

            ClearConsole(BACKGROUND_GREEN);
            Console.SetCursorPosition(0, (Console.WindowHeight / 2) - 8);
            Console.WriteLine(text.PastelBg(BACKGROUND_GREEN));
            for (var i = 0; i <= barLength; i++)
            {
                var fillPart = new string(' ', i).PastelBg(DARK_BROWN);
                var emptyPart = new string(' ', barLength - i).PastelBg(BROWN);
                Console.SetCursorPosition(0, Console.WindowHeight / 2);
                Console.WriteLine($"{offset}{fillPart}{emptyPart} {i,3}%".PastelBg(BACKGROUND_GREEN));
                Thread.Sleep(delay);
            }
            countdownSound.Stop();
        }

        private static void AboutTab()
        {
            var title = FiggleFonts.Broadway.Render("ABOUT");
            var text = $@"
This application was developed as part of an academic project for the User Interface (UI) subject.

{"\x1b[1m\x1b[4mResources\x1b[0m"}:

1. {"https://paulbourke.net/dataformats/asciiart/".Pastel(BLUE)} (ASCII Characters)
2. {"https://ascii.co.uk/art/snake".Pastel(BLUE)} (Jennifer E. Swofford)
3. {"https://www.pngegg.com/en/png-ogczk".Pastel(BLUE)} (Snake image)
4. {"https://mixkit.co/free-sound-effects/".Pastel(BLUE)} (Sounds)
5. {"https://getemoji.com/".Pastel(BLUE)} (Emoji)
6. {"https://www.i2symbol.com/symbols/square".Pastel(BLUE)} (Emocji)

{"[Press any key to return back to the menu.]".Pastel(DARK_BLUE)}";

            ClearConsole(BACKGROUND_BLUE);
            Console.WriteLine(title.PastelBg(BACKGROUND_BLUE));
            Console.WriteLine(text.PastelBg(BACKGROUND_BLUE));

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
                var color = (i == selectedIndex) ? (option == Tab.Exit ? RED : GREEN) : WHITE;
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
                Console.WriteLine(title.Pastel(color).PastelBg(BACKGROUND_GREEN));
                Console.WriteLine(text.Pastel(DARK_BROWN).PastelBg(BACKGROUND_GREEN));
                Thread.Sleep(delay);
            }
        }

        private static void DisplayImage()
        {
            var ratio = 2 * image.Width / image.Height; // char size is 16 px wide x 8 px hight
            var height = Console.WindowHeight - 8; // 8 is menu size
            var width = height * ratio;

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
                        var coloredValue = asciiValue.Pastel(Color.FromArgb(pixelColor.R, pixelColor.G, pixelColor.B)).PastelBg(BACKGROUND_GREEN);
                        Console.Write(coloredValue);
                    }
                    Console.WriteLine();
                }
                menuTopPosition = ++height;
            }
        }

        private static void LoadSounds()
        {
            countdownSound.Load();
            gameOverSound.Load();
            introSound.Load();
            pickSound.Load();
            selectSound.Load();
        }

        private static void ClearConsole(string color)
        {
            Console.Clear();
            var emptyLine = new string(' ', Console.WindowWidth).PastelBg(color);
            for (var i = 1; i < Console.BufferHeight; i++)
            {
                Console.WriteLine(emptyLine);
            }
            Console.SetCursorPosition(0, 0);
        }

        private static void ClearConsoleAtLine(int line)
        {
            Console.SetCursorPosition(0, line);
            Console.WriteLine(new string(' ', Console.WindowWidth).PastelBg(BACKGROUND_GREEN));
        }

        private static void SetupConsole()
        {
            Console.Title = CONSOLE_TITLE;
            Console.OutputEncoding = Encoding.UTF8;
            Console.CursorVisible = false;
        }
    }
}
