namespace SceneServices.TagCategories
{
    public sealed class Attribute : Category
    {
        public static Attribute Instance => new Attribute();
        public override TagCategoryTypes Type => TagCategoryTypes.Attribute;

        public static readonly string LegsTied = "LEGSTIED";
        public static readonly string Bound = "BOUND";
        public static readonly string Cuffed = "CUFFED";

        public override string[] Tags
        {
            get
            {
                return new[]
                {
                    "ANAL",
                    "ORAL",
                    Bound,
                    "DEATH",
                    "PRONE",
                    "TEASE",
                    "CAUGHT",
                    Cuffed,
                    "FEMDOM",
                    "GAGGED",
                    "CLIMAXM",
                    "SWALLOW",
                    "UPRIGHT",
                    "FACEDOWN",
                    "FLEXIBLE",
                    "KNEELING",
                    "LEGSFREE",
                    LegsTied,
                    "POUNDING",
                    "SIDEWAYS",
                    "SPOONING",
                    "STANDING",
                    "PERPENDICULAR",
                    //"LEFTHANDTOBUTT",
                    "REVCOWGIRL",
                    "FROMBACK",
                    "FROMSIDE",
                    "FROMFRONT",
                    "FROMBEHIND",
                    "TENDER",
                };
            }
        }
    }
}