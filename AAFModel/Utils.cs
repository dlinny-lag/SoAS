namespace AAFModel
{
    public static class Utils
    {
        /// <summary>
        /// updates string values with their interned values
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string[] Intern(this string[] lines)
        {
            for (int i = 0; i < lines.Length; i++)
            {
                lines[i] = string.Intern(lines[i]);
            }

            return lines;
        }
    }
}