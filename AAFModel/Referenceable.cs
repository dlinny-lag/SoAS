namespace AAFModel
{
    public abstract class Referenceable : Declared
    {
        protected Referenceable(string file, int order) : base(file, order)
        {
        }

        public abstract ReferenceType Type { get; }
    }
}