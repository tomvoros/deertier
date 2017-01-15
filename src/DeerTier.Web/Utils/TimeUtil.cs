using DeerTier.Web.Objects;
using System.Text.RegularExpressions;

namespace DeerTier.Web.Utils
{
    public class TimeUtil
    {
        public static FormattedTime GetFormattedTime(string timeString)
        {
            Match match = new Regex("^(\\d\\d?):(\\d\\d):(\\d\\d?)$").Match(timeString);
            if (match.Success)
            {
                int num = int.Parse(match.Groups[1].Value);
                int num2 = int.Parse(match.Groups[2].Value);
                int num3 = int.Parse(match.Groups[3].Value);
                int timeSeconds = num3 + num2 * 60 + num * 60 * 60;
                string text = num.ToString();
                string text2 = num2.ToString();
                string text3 = num3.ToString();
                if (num < 10)
                {
                    text = "0" + text;
                }
                if (num2 < 10)
                {
                    text2 = "0" + text2;
                }
                if (num3 < 10)
                {
                    text3 = "0" + text3;
                }
                string timeString2 = string.Concat(new string[]
                {
                    text,
                    ":",
                    text2,
                    ":",
                    text3
                });
                if (num < 0 || num2 < 0 || num3 < 0 || num2 >= 60 || num3 >= 60)
                {
                    return new FormattedTime(-1, "-1");
                }
                return new FormattedTime(timeSeconds, timeString2);
            }
            else
            {
                match = new Regex("^(\\d\\d?):(\\d\\d)$").Match(timeString);
                if (!match.Success)
                {
                    return new FormattedTime(-1, "-1");
                }
                int num4 = int.Parse(match.Groups[1].Value);
                int num5 = int.Parse(match.Groups[2].Value);
                int timeSeconds = num5 + num4 * 60;
                string text4 = num4.ToString();
                string text5 = num5.ToString();
                if (num4 < 10)
                {
                    text4 = "0" + text4;
                }
                if (num5 < 10)
                {
                    text5 = "0" + text5;
                }
                string timeString3 = text4 + ":" + text5;
                if (num4 < 0 || num5 < 0 || num4 >= 60 || num5 >= 60)
                {
                    return new FormattedTime(-1, "-1");
                }
                return new FormattedTime(timeSeconds, timeString3);
            }
        }
    }
}