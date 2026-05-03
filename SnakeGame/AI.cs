using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http;
using System.Security.Policy;
using System.Text;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Spectre.Console;
using static SnakeGame.Auth;
using static SnakeGame.Enum;
using static SnakeGame.Parameters;
using static SnakeGame.Sounds;

namespace SnakeGame
{
    internal class AI
    {

        internal static async Task<string> Call(string text)
        {
            var prompt = CreatePrompt(text);
            var requestBody = GetRequestBody(prompt);

            using (var client = new HttpClient())
            {
                client.DefaultRequestHeaders.Add("x-goog-api-key", GEMINI_API_KEY);
                var json = JsonConvert.SerializeObject(requestBody);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await client.PostAsync(
                    $"{GEMINI_URL}/{MODEL}:generateContent",
                    content
                );
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStringAsync();
                var result = ExtractText(responseBody);
                Console.WriteLine(result);

                return result;
            }
        }

        private static object GetRequestBody(string text)
        {
            return new
            {
                contents = new[]
                {
                new
                {
                    parts = new[]
                    {
                        new { text = text }
                    }
                }
            }
            };
        }

        private static string ExtractText(string responseBody)
        {
            var result = JsonConvert.DeserializeObject<GeminiResponse>(responseBody);

            if (result == null || result.Candidates == null || result.Candidates.Count == 0)
                return string.Empty;

            var candidate = result.Candidates[0];

            if (candidate.Content == null || candidate.Content.Parts == null || candidate.Content.Parts.Count == 0)
                return string.Empty;

            return candidate.Content.Parts[0].Text ?? string.Empty;
        }

        public class GeminiResponse
        {
            [JsonProperty("candidates")]
            public List<Candidate> Candidates { get; set; }
        }

        public class Candidate
        {
            [JsonProperty("content")]
            public Content Content { get; set; }
        }

        public class Content
        {
            [JsonProperty("parts")]
            public List<Part> Parts { get; set; }
        }

        public class Part
        {
            [JsonProperty("text")]
            public string Text { get; set; }
        }

        private static string CreatePrompt(string text)
        {
            return $@"
You are a Snake game configurator. Extract game settings from the user's description: ""{text}"".

Return ONLY a valid JSON object — no markdown, no extra text, no wrapers — with these exact fields:
{{
  ""snakeColor"": ""White"" | ""Green"" | ""Violet"",
  ""boardColor"": ""Dim"" | ""Normal"" | ""Bright"",
  ""boardHeight"": integer 15-28,
  ""boardWidth"": integer 15-26,
  ""fruitsNumber"": integer 1-9,
  ""speed"": ""Slow"" | ""Medium"" | ""Fast""
}}

For any setting not mentioned, pick a sensible value. 
Defaults are: Green color, Normal board, 20, 25, 5 fruits, and Medium speed.
Square boards have equal height and width.

Return ONLY the JSON object, nothing else.
";
        }
    }
}