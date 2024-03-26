using System;

namespace AAF.Services.Differences
{
    [Flags]
    public enum StringDifference
    {
        None,
        Content = 1,
    }
    public sealed class StringsComparer : IElementComparer<string, StringDifference>
    {
        public static readonly StringsComparer Default = new StringsComparer();
        public StringDifference Same(string a, string b)
        {
            if (a != b) // do not ignore case
                return StringDifference.Content;
            return StringDifference.None;
        }
    }
}