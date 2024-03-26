using System;
using System.Text;

namespace Shared.Controls
{
    public static class RtfHelper
    {
        private static string GetRtfEncoding(char c)
        {
            if (c == '\\') return "\\\\";
            if (c == '{') return "\\{";
            if (c == '}') return "\\}";
            int intCode = Convert.ToInt32(c);
            if (char.IsLetter(c) && intCode < 0x80)
            {
                return c.ToString();
            }
            return "\\u" + intCode + "?";
        }
        public static string GetRtfString(this string s)
        {
            StringBuilder returned = new StringBuilder();
            foreach(char c in s)
            {
                returned.Append(GetRtfEncoding(c));
            }
            return returned.ToString();
        }
    }
}