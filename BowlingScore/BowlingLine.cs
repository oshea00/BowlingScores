using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace BowlingScore
{
    public class BowlingLine
    {
        public List<Frame> Frames { get; }
        public BowlingLine(string bowlingLine)
        {
            try
            {
                Frames = ParseLine(bowlingLine);
            }
            catch (Exception)
            {
                throw new Exception($"Invalid Bowling Line String '{bowlingLine}'");
            }
        }

        List<Frame> ParseLine(string bowlingLine)
        {
            var line = Regex.Replace(bowlingLine, @"(\|)\1", "$1");
            var frametoks = line.Split("|");
            frametoks[9] = frametoks[9] + frametoks[10];
            if (frametoks.Length == 11)
            {
                return frametoks.Take(10).Select(f => new Frame(f)).ToList();
            }
            else
            {
                throw new Exception($"Invalid Bowling Line String '{line}'");
            }
        }

        public int ScoreGame()
        {
            // Patterns:
            // X

            int score = 0;
            try {
                for (int i = 0; i < Frames.Count; i++)
                {
                    var fscore = Frames[i].Score;
                    if (fscore.bonus == 0) // OPEN FRAME
                    {
                        score += fscore.score;
                    }

                    if (fscore.bonus == 1) // SPARE
                    {
                        var frame2 = Frames[i + 1];
                        if (frame2.First < Frame.ThrowValue.STRIKE)
                        {
                            score += fscore.score + (int)frame2.First;
                        }
                        else
                        {
                            score += frame2.Score.score;
                        }
                    }

                    if (fscore.bonus == 2) // STRIKE
                    {
                        if (Frames[i + 1].BonusThrow != Frame.ThrowValue.EMPTY) // 9th Frame STRIKE
                        {
                            // 10th Frame  X44
                            if (Frames[i + 1].First == Frame.ThrowValue.STRIKE &&
                                Frames[i + 1].Second < Frame.ThrowValue.STRIKE)
                                score += fscore.score + (int)Frames[i + 1].First + (int)Frames[i + 1].Second;
                            else
                            // 10th Frame 4/3
                            if (Frames[i + 1].Second == Frame.ThrowValue.SPARE)
                                score += 20;
                            else
                                // 10th Frame 44
                                score += fscore.score + (int)Frames[i + 1].First + (int)Frames[i + 1].Second;
                        }
                        else
                        if (Frames[i + 1].First == Frame.ThrowValue.STRIKE &&
                            Frames[i + 2].First == Frame.ThrowValue.STRIKE)
                            score += fscore.score + 20;
                        else
                        if (Frames[i + 1].Second == Frame.ThrowValue.SPARE)
                            score += fscore.score + 10;
                        else
                            score += fscore.score + (int)Frames[i + 1].First + (int)Frames[i + 1].Second; 
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Game Score Error",ex);
            }
            return score;
        }
    }
}
