namespace SceneServices.TagCategories
{
    public sealed class Service : Category
    {
        public static Service Instance => new Service();
        public override TagCategoryTypes Type => TagCategoryTypes.Service;

        public override string[] Tags
        {
            get
            {
                return new[]
                {
                    "TD", // torture devices
                    "START",
                    "CLIMAX",
                    "FINISH",
                    "HIDDEN",
                    "MALECUM",
                    "UTILITY",
                    "MIDSTAGE",
                    "STAGESET",
                    "SEQUENTIAL"
                };
            }
        }
    }
}