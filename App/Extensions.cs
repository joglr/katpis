using System.Collections.Generic;

namespace App
{
  public static class Extensions {
        public enum Style {
            Black,
            Red,
            Green,
            Yellow,
            Blue,
            Magenta,
            Cyan,
            White,
            ResetColor,
            Bold,
            Underlined,
            ResetStyle
        }
        public static Dictionary<Style, string> StyleANSI = new Dictionary<Style, string>
        {
            { Style.Black, "\u001b[30m" },
            { Style.Red, "\u001b[31m" },
            { Style.Green, "\u001b[32m" },
            { Style.Yellow, "\u001b[33m" },
            { Style.Blue, "\u001b[34m" },
            { Style.Magenta, "\u001b[35m" },
            { Style.Cyan, "\u001b[36m" },
            { Style.White, "\u001b[37m" },
            { Style.ResetColor, "\u001b[0m" },

            { Style.Bold, "\x1b[1m" },
            { Style.ResetStyle, "\x1b[0m" }
        };

        public static string Black(this string str)
        {
            return StyleANSI[Style.Black] + str + StyleANSI[Style.ResetColor];
        }

        public static string Red(this string str)
        {
            return StyleANSI[Style.Red] + str + StyleANSI[Style.ResetColor];
        }

        public static string Green(this string str)
        {
            return StyleANSI[Style.Green] + str + StyleANSI[Style.ResetColor];
        }

        public static string Yellow(this string str)
        {
            return StyleANSI[Style.Yellow] + str + StyleANSI[Style.ResetColor];
        }

        public static string Blue(this string str)
        {
            return StyleANSI[Style.Blue] + str + StyleANSI[Style.ResetColor];
        }

        public static string Magenta(this string str)
        {
            return StyleANSI[Style.Magenta] + str + StyleANSI[Style.ResetColor];
        }

        public static string Cyan(this string str)
        {
            return StyleANSI[Style.Cyan] + str + StyleANSI[Style.ResetColor];
        }

        public static string White(this string str)
        {
            return StyleANSI[Style.White] + str + StyleANSI[Style.ResetColor];
        }

        public static string Bold(this string str)
        {
            return StyleANSI[Style.Bold] + str + StyleANSI[Style.ResetStyle];
        }
    }
}
