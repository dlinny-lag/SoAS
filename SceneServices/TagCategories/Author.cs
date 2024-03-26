namespace SceneServices.TagCategories
{
    public sealed class Author : Category
    {
        public static Author Instance => new Author();
        public override TagCategoryTypes Type => TagCategoryTypes.Author;

        public override string[] Tags
        {
            get
            {
                return new[]
                {
                    "ZAZ",
                    "BP70",
                    "RP70",
                    "BRAVE",
                    "CRAZY",
                    "LEITO",
                    "RUFGT",
                    "RUGFT",
                    "VADER",
                    "AVILAS",
                    "FARELLE",
                    "ROHZIMA",
                    "GRAYUSER",
                    "VADER666",
                    "RAINMAKER",
                    "SAVAGECABBAGE",

                    "SABA", // not actually an author, but S means spicy while author is spicydoritos
                    "SACA", // not actually an author, but S means spicy while author is spicydoritos
                };
            }
        }
    }
}