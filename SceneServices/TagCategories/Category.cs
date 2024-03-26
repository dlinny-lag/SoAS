namespace SceneServices.TagCategories
{
    public abstract class Category
    {
        public abstract TagCategoryTypes Type { get; }
        public abstract string[] Tags { get; }
    }
}