namespace SceneServices.TagCategories
{
    public sealed class Narrative : Category
    {
        #region BadEnd
        public static readonly string NeckHanging = string.Intern("Hung".ToUpperInvariant());
        public static readonly string NeckStrangling = string.Intern("Strangled".ToUpperInvariant());
        #endregion

        public static Narrative Instance => new Narrative();
        public override TagCategoryTypes Type => TagCategoryTypes.Narrative;
        public override string[] Tags
        {
            get
            {
                return new[]
                {
                    "69",
                    "BJ",
                    "FJ",
                    NeckHanging,
                    "POSE",
                    "CARRY",
                    "DANCE",
                    "DOGGY",
                    "SPOON",
                    "CUDDLE",
                    "RIMJOB",
                    "TERASE", // mistype of tease
                    "TITJOB",
                    "URINAL",
                    "BLOWJOB",
                    "BOOBJOB",
                    "COWGIRL",
                    "DANCING",
                    "FISTING",
                    "FOOTJOB",
                    "HANDJOB",
                    "HUGGING",
                    "IMPALED",
                    "JACKOFF",
                    "KISSING",
                    "RIMMING",
                    "SCISSOR",
                    "SITTING",
                    "FACEFUCK",
                    "FOOTPLAY",
                    "FOREPLAY",
                    "GANGBANG",
                    "SPANKING",
                    "ANALINGUS",
                    "FINGERING",
                    "LICKTEASE",
                    "POWERBOMB",
                    "SEXYDANCE",
                    "SPITROAST",
                    NeckStrangling,
                    "DOUBLETEAM",
                    "MISSIONARY",
                    "SCISSORING",
                    "CUNNILINGUS",
                    "FACESITTING",
                    "ELECTROCUTED",
                    "MASTURBATION",
                    "SWITCH",
                };
            }
        }
    }
}