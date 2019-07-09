using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Linq;

namespace BowlingScore
{
    public class FrameStringValidator
    {
        static Regex valid2 = new Regex(@"^(\d\/)|^(\d\d)");
        static Regex valid3 = new Regex(@"^(\d\/X)|^(\d\/\d)|^(XXX)|^(X\d\/)|^(X\d\d)|^(XX\d)");
        static Regex num2 = new Regex(@"\d\d");
        static Regex num3 = new Regex(@"X\d\d");

        static bool ValidScore(string f)
        {
            var adigit = int.TryParse(f.Substring(0,1), out int a);
            var bdigit = int.TryParse(f.Substring(1,1), out int b);
            return a + b <= 9;
        }

        static bool IsStrike(string frameString)
        {
            if (frameString.Length == 1 && frameString.First() == 'X')
                return true;
            return false;
        }

        public static bool IsValid(string frameString)
        {
            if (IsStrike(frameString))
                return true;

            if (frameString.Length == 2 && valid2.Match(frameString).Success)
            {
                if (num2.Match(frameString).Success && 
                    !ValidScore(frameString))
                    return false;
                return true;
            }

            if (frameString.Length == 3 && valid3.Match(frameString).Success)
            {
                if (num3.Match(frameString).Success && 
                    !ValidScore(frameString.Substring(1,2)))
                    return false;
                return true;
            }

            return false;
        }
    }
}
