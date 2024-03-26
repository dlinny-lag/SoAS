namespace AAFModel
{
    public sealed class Race : Declared
    {
        public string Skeleton { get; set; }
        public int FormId { get; set; }

        public Race(string file, int order) : base(file, order)
        {
        }
    }
}