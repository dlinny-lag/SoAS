using System;
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

        public static string ValidateFilename(this string file)
        {
            if (file == null)
                throw new ArgumentNullException(nameof(file));
            
            if (file.IsValidFilename())
                return file;

            for (int i = 0; i < UnallowedChars.Length; i++)
            {
                file = file.Replace(UnallowedChars[i], '_');
            }
            return file;
        }
    }
}