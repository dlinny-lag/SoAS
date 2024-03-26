namespace AAFModel
{
    /// <summary>
    /// <see cref="Declared.Id"/> is used as PositionId
    /// </summary>
    public sealed class TagsEntry : Declared
    {
        public string[] Tags { get; set; }

        public TagsEntry(string file, int order) : base(file, order)
        {
        }
    }
}