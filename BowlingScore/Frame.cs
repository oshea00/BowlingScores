using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace BowlingScore
{
    public class Frame
    {
        public enum ThrowValue { ZERO, ONE, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, STRIKE, SPARE,EMPTY};
        public ThrowValue First { get; set; }
        public ThrowValue Second { get; set; }
        public ThrowValue BonusThrow { get; set; }
        public (int score, int bonus) Score
        {
            get {
                if (BonusThrow == ThrowValue.EMPTY)
                {
                    if (First == ThrowValue.STRIKE)
                        return (10, 2);
                    if (First < ThrowValue.STRIKE && Second < ThrowValue.STRIKE)
                        return ((int)First + (int)Second, 0);
                    if (Second == ThrowValue.SPARE)
                        return (10, 1);
                }
                else
                {
                    if (First == ThrowValue.STRIKE && Second == ThrowValue.STRIKE)
                        return (20 + (int)BonusThrow, 0);
                    if (First < ThrowValue.STRIKE && Second == ThrowValue.SPARE)
                        return (10 + (int)BonusThrow, 0);
                    if (First == ThrowValue.STRIKE && Second < ThrowValue.STRIKE &&
                        BonusThrow == ThrowValue.SPARE)
                        return (20, 0);
                    if (First == ThrowValue.STRIKE && Second < ThrowValue.STRIKE && 
                        BonusThrow < ThrowValue.STRIKE)
                        return (10 + (int)Second + (int)BonusThrow, 0);
                }
                throw new Exception("Can't Score Frame");
            }
        }

        public Frame()
        {
            First = ThrowValue.EMPTY;
            Second = ThrowValue.EMPTY;
            BonusThrow = ThrowValue.EMPTY;
        }

        public Frame(string value) : this()
        {
            value = value.Replace("-", "0"); // allow '-' to mean '0'
            if (FrameStringValidator.IsValid(value))
            {
                var len = value.Length;
                if (len>=1)
                    First = GetBall(value, 1);
                if (len>=2)
                    Second = GetBall(value, 2);
                if (value.Length == 3)
                    BonusThrow = GetBall(value, 3);
            } else
            {
                throw new Exception($"Invalid Frame String: '{value}'");
            }
        }

        ThrowValue GetBall(string frame, int ball)
        {
            switch (frame.Substring(ball-1,1))
            {
                case "X": return ThrowValue.STRIKE;
                case "/": return ThrowValue.SPARE;
                case "0": return ThrowValue.ZERO;
                case "1": return ThrowValue.ONE;
                case "2": return ThrowValue.TWO;
                case "3": return ThrowValue.THREE;
                case "4": return ThrowValue.FOUR;
                case "5": return ThrowValue.FIVE;
                case "6": return ThrowValue.SIX;
                case "7": return ThrowValue.SEVEN;
                case "8": return ThrowValue.EIGHT;
                case "9": return ThrowValue.NINE;
                default: return ThrowValue.EMPTY;
            }
        }
    }

}
