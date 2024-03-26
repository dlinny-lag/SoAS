using System.Collections.Generic;
using System.Linq;
using Shared.Utils;

namespace SceneServices.TagCategories
{
    public static class ActorTypes
    {
        private static readonly char[] Separator = "_".ToCharArray();

        private static readonly HashSet<string> knownSingle = new HashSet<string>
        {
            "P",
            "M",
            "F",
            "2P",
            "3P",
            "4P",
            "5P",
            "6P",
            "7P",
            "8P",
            "9P",
            "10P",
        };

        private static readonly HashSet<string> knownParts = new HashSet<string>
        {
            "P",
            "M",
            "F",
            "DOGM",
            "SMUTANTM",
            "FGHOULM",
            "DCLAWM",
            "DCLAW",
            "FEVHOUNDM",
            "GORILLAM",
            "CATM",
            "CATF",
            "MIRELURK",
            "MIRELURKH",
            "MIRELURKK",
            "MIRELURKQU",
            "SCORP",
            "YAOGUAI",
            "BLDBUG",
            "BLTFLY",
            "STGWING",
            "SMBEHEMOTH",
            "ANGLER",
            "ALIENM",
            "GULPER",
            "HERMIT",
            "FOGCRAWLER",
            "PROTECTRONM",
            "HANDYM",
            "MOLERAT",
            "BRAHMIN",
            "BLTGHOULM",
        };

        public static bool IsActorTypes(this string tag)
        {
            string[] parts = tag.Split(Separator);

            if (parts.Length == 1)
                return parts[0].IsIn(knownSingle);

            return parts.All(part => part.IsIn(knownParts));
        }
    }
}