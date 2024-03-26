using System.Collections.Generic;
using System.IO;

namespace Shared.Utils
{
    public static class FilenameValidation
    {
        private static readonly char[] UnallowedChars;

        static FilenameValidation()
        {
            List<char> unallowed = new List<char>(Path.GetInvalidFileNameChars());
            unallowed.Add(',');
            UnallowedChars = unallowed.ToArray();
        }
        public static bool IsValidFilename(this string file)
        {
            return file.IndexOfAny(UnallowedChars) < 0;
        }
    }
}