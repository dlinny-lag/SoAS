using System;

namespace AAF.Services.AAFImport
{
    public static class SexHelper
    {
        private static class EnumConstants<TS>
            where TS: Enum
        {
            public static readonly TS Any = (TS)(object)0;
            public static readonly TS Male = (TS)(object)1;
            public static readonly TS Female = (TS)(object)2;
        }

        public static TS ToSex<TS>(this string gender)
            where TS: Enum
        {
            if (string.IsNullOrWhiteSpace(gender))
                return EnumConstants<TS>.Any;
            if (gender.IsMale())
                return EnumConstants<TS>.Male;
            if (gender.IsFemale())
                return EnumConstants<TS>.Female;
            return EnumConstants<TS>.Any;
        }

        private static bool IsMale(this string gender)
        {
            if (string.Compare("M", gender, StringComparison.InvariantCultureIgnoreCase) == 0)
                return true;
            if (string.Compare("male", gender, StringComparison.InvariantCultureIgnoreCase) == 0)
                return true;
            return false;
        }
        private static bool IsFemale(this string gender)
        {
            if (string.Compare("F", gender, StringComparison.InvariantCultureIgnoreCase) == 0)
                return true;
            if (string.Compare("female", gender, StringComparison.InvariantCultureIgnoreCase) == 0)
                return true;
            return false;
        }
    }
}