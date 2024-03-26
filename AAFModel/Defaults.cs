namespace AAFModel
{
    public class Defaults
    {
        public static Defaults Default => new Defaults();
        public int Priority { get; set; } = 1;
        public string IdleSource { get; set; }
        public string Source { get; set; }
        public string Skeleton { get; set; } = "Human";
    }
}