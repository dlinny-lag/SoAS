namespace SceneServices.TagCategories
{
    public sealed class Feeling : Category
    {
        public override TagCategoryTypes Type => TagCategoryTypes.Feeling;

        public static readonly string Loving = string.Intern("Loving".ToUpperInvariant());
        public static readonly string Neutral = string.Intern("Neutral".ToUpperInvariant());
        public static readonly string Aggressive = string.Intern("Aggressive".ToUpperInvariant());
        public static readonly string Rough = string.Intern("Rough".ToUpperInvariant());

        public static Feeling Instance => new Feeling();

        public override string[] Tags
        {
            get
            {
                return new[]
                {
                    Loving,
                    Neutral,
                    Aggressive,
                    Rough,
                };
            }
        }
    }
}