namespace AAFModel
{
    public abstract class Declared
    {
        protected Declared(string file, int order)
        {
            File = file;
            Order = order;
        }
        public readonly string File;
        public readonly int Order;

        public string Id { get; set; }
    }
}