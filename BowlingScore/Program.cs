using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BowlingScore
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var p = new Program();
            p.ScoreGames();
            Console.ReadKey();
        }

        void ScoreGames()
        {
            string[] games = new[] {
                "X|X|X|X|X|X|X|X|X|X||XX",  // 300 - perfect game
		        "X|15|1/|X|9/|X|X|X|X|X||12", // 206
		        "X|1/|X|5/|X|3/|X|8/|X|9/||X", // 200 - "scottish" 200
		        "X|1/|X|5/|X|3/|X|8/|X|--||", // 170
                "16|5/|54|3/|5/|X|X|X|6/|X||X5", // 187
                "bad game", // Invalid
                null, // Invalid
                "", // Invalid
                "16|5/|54|XX|5/|X|X|X|6/|X||X5" // Invalid
  	        };

            foreach (var game in games)
            {
                try
                {
                    var score = (new BowlingLine(game)).ScoreGame();
                    Console.WriteLine($"Game: {game} Score: {score}");
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }
    }

}
