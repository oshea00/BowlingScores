using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace BowlingScore
{
    class Program
    {
        public static void Main(string[] args)
        {
            var p = new Program();
            p.Main();
            Console.ReadKey();
        }

        void Main()
        {
            var games = new[] {
        "X|X|X|X|X|X|X|X|X|X||XX",  // 300 - perfect game
		"X|15|1/|X|9/|X|X|X|X|X||12", // 206
		"X|1/|X|5/|X|3/|X|8/|X|9/||X1/", // 200 - "scottish" 200
		"X|1/|X|5/|X|3/|X|8/|X|--||", // 170
	};
            foreach (var game in games)
            {
                var v = ValidGameLine(game);
                if (v.valid)
                {
                    Console.WriteLine($"Game: {game} Score: {ScoreGame(v.frames)}");
                }
                else
                {
                    Console.WriteLine($"Game: {game} Score: INVALID");
                }
            }
        }

        // Methods
        List<(int score, int bonus)> ScoreFrames(string[] frames)
        {
            var scores = new List<(int score, int bonus)>();
            for (var i = 0; i < 10; i++)
            {
                if (frames[i].First() == 'X')
                {
                    if (frames[i + 1].Last() == '/')
                    {
                        scores.Add((10, 1));
                    }
                    else
                    {
                        scores.Add((10, 2));
                    }
                }
                else if (frames[i].Last() == '/')
                {
                    scores.Add((10, 1));
                }
                else
                {
                    scores.Add((int.Parse(frames[i].Substring(0, 1)), 0));
                    scores.Add((int.Parse(frames[i].Substring(1)), 0));
                }
            }
            var bonus = frames.Last();
            if (bonus.Length == 0)
            {
                scores.Add((0, 0));
                scores.Add((0, 0));
            }
            else
            if (bonus == "XX")
            {
                scores.Add((10, 0));
                scores.Add((10, 0));
            }
            else if (bonus.First() == 'X')
            {
                scores.Add((10, 1));
                scores.Add((int.Parse(bonus.Substring(1)), 0));
            }
            else if (bonus.Last() == '/')
            {
                scores.Add((10, 0));
                scores.Add((0, 0));
            }
            else
            {
                scores.Add((int.Parse(bonus.Substring(0, 1)), 0));
                scores.Add((int.Parse(bonus.Substring(1)), 0));
            }
            return scores;

        }

        int ScoreGame(string[] frames)
        {
            var total = 0;
            var scores = ScoreFrames(frames);
            for (int i = 0; i < scores.Count() - 2; i++)
            {
                switch (scores[i].bonus)
                {
                    case 0:
                        total += scores[i].score;
                        break;
                    case 1:
                        total += (scores[i].score + scores[i + 1].score);
                        break;
                    case 2:
                        total += (scores[i].score + scores[i + 1].score + scores[i + 2].score);
                        break;
                }
            }
            return total;
        }

        bool ValidFrame(string frame)
        {
            // X n/ -- -n n-
            if (frame.Length < 1 || frame.Length > 2)
                return false;
            if (frame.Length == 2 && (frame.First() == 'X' || frame.First() == '/' || frame.Last() == 'X'))
                return false;
            if (frame.Length == 1 && frame.First() != 'X')
                return false;
            if (int.TryParse(frame, out var total))
            {
                // frame contains only digits
                if (total / 10 + total % 10 > 9)
                    return false;
            }
            return true;
        }

        bool ValidBonusFrame(string frame)
        {
            // XX Xn X n/ n0 0n nn
            if (frame.Length > 2 || frame.Length == 1)
                return false;
            if (frame.Length == 2)
            {
                if (frame != "XX")
                {
                    if (int.TryParse(frame, out var total))
                    {
                        if (total / 10 + total % 10 > 9)
                            return false;
                    }
                    else
                    {
                        if (frame.First() != 'X')
                            return false;
                    }
                }
            }
            return true;
        }

        (bool valid, string[] frames) ValidGameLine(string line)
        {
            // line contains only valid characters
            string valid = "123456789-X/|";
            if (line.Any(l => (valid.Contains(l) == false)))
            {
                return (false, null);
            }
            // valid frame count/syntax (can still have invalid semantics - such as 99)
            var pattern = @"([X]{1}\|)|([1-9\-]{1}[1-9\-/]{1}\|)|([\|]{1}[X\-1-9/]{0,2})";
            var regx = new Regex(pattern);
            var match = regx.Matches(line);
            if (match.Count != 11)
                return (false, null);

            var frames = match.Cast<Match>()
                              .Select(m => m.Value.Replace("|", "").Replace("-", "0"))
                              .ToArray();

            // frames 1-10 valid
            if (frames.Take(10).Any(f => ValidFrame(f) == false))
                return (false, null);
            // Last frame valid
            if (ValidBonusFrame(frames.Last()) == false)
                return (false, null);
            return (true, frames);
        }
    }

}
