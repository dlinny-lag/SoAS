namespace SceneModel
{
    public enum Sex
    {
        Any = 0,
        Male = 1,
        Female = 2,
        Both = 3
    }

    public static class SexExtension
    {
        private static readonly string[] map = new string[4] { "P", "M", "F", "A" };
        public static string Abbr(this Sex sex)
        {
            return map[(int)sex];
        }
    }
}