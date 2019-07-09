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
            int score = 0;
            try {
                for (int i = 0; i < Frames.Count; i++)
                {
                    var fscore = Frames[i].Score;

                    if (fscore.bonus == 0) // OPEN FRAME OR 10th FRAME
                        score += fscore.score;

                    if (fscore.bonus == 1) // SPARE
                        score += fscore.score + (int)Frames[i + 1].First;

                    if (fscore.bonus == 2) // STRIKE
                    {
                        if (i<8) // Frames 1-8
                        {
                            // if strikes in next two frames
                            if (Frames[i + 1].First == Frame.ThrowValue.STRIKE &&
                                Frames[i + 2].First == Frame.ThrowValue.STRIKE)
                                score += fscore.score + 20;
                            else
                            // if spare in next frame
                            if (Frames[i + 1].Second == Frame.ThrowValue.SPARE)
                                score += fscore.score + 10;
                            // next frame open or STRIKE
                            else
                                score += Frames[i + 1].Score.score;
                        }
                        else
                        if (i==8) // 9th frame STRIKE
                        {
                            // 10th XXn
                            if (Frames[i + 1].First == Frame.ThrowValue.STRIKE &&
                                Frames[i + 1].Second == Frame.ThrowValue.STRIKE)
                                score += fscore.score + 20;
                            else
                            // 10th Xnn or Xn/
                            if (Frames[i + 1].First == Frame.ThrowValue.STRIKE &&
                                Frames[i + 1].Second < Frame.ThrowValue.STRIKE)
                                score += fscore.score + (int)Frames[i + 1].Second;
                            else
                            // 10th n/n
                            if (Frames[i + 1].First < Frame.ThrowValue.STRIKE &&
                                Frames[i + 1].Second == Frame.ThrowValue.SPARE)
                                score += fscore.score + 10;
                            // 10th Open
                            else
                                score += Frames[i + 1].Score.score;
                        }
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
