using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Spectre.Console;
using static SnakeGame.Parameters;
using static SnakeGame.Sounds;

namespace SnakeGame
{
    // GameSettings DTO
    internal class GameSettings
    {
        public string SnakeColor { get; set; } = "Green";
        public string BoardColor { get; set; } = "Dim";
        public int BoardHeight { get; set; } = 20;
        public int BoardWidth { get; set; } = 25;
        public int FruitsNumber { get; set; } = 5;
        public string Speed { get; set; } = "Fast";
    }
}