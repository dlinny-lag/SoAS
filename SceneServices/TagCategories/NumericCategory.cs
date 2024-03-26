using System;
using System.Globalization;

namespace SceneServices.TagCategories
{
    public enum NumericType
    {
        None = 0,
        Stim,
        Dom,
        Held,
        Love,
    }

    public struct NumericValue
    {
        public NumericType Type;
        public int Value;
    }

    public static class NumericCategory
    {
        public static readonly string StimPrefix = "Stim".ToUpperInvariant();
        public static readonly string DomPrefix = "Dom".ToUpperInvariant();
        public static readonly string HeldPrefix = "Held".ToUpperInvariant();
        public static readonly string LovePrefix = "Love".ToUpperInvariant();

        public static NumericValue AsNumeric(this string tag)
        {
            NumericValue retVal = new NumericValue{Value = 0,Type = NumericType.None};
            tag = tag.ToUpperInvariant();
            NumericType type = tag.ExtractType(out var index);
            if (type == NumericType.None)
                return retVal;
            string val = tag.Substring(index);
            if (!int.TryParse(val, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int value))
                return retVal;
            retVal.Type = type;
            retVal.Value = value;
            return retVal;
        }

        private static NumericType ExtractType(this string tag, out int index)
        {
            int ind = tag.IndexOf(StimPrefix, StringComparison.InvariantCulture);
            if (ind == 0)
            {
                index = StimPrefix.Length;
                return NumericType.Stim;
            }

            ind = tag.IndexOf(DomPrefix, StringComparison.InvariantCulture);
            if (ind == 0)
            {
                index = DomPrefix.Length;
                return NumericType.Dom;
            }

            ind = tag.IndexOf(HeldPrefix, StringComparison.InvariantCulture);
            if (ind == 0)
            {
                index = HeldPrefix.Length;
                return NumericType.Held;
            }

            ind = tag.IndexOf(LovePrefix, StringComparison.InvariantCulture);
            if (ind == 0)
            {
                index = LovePrefix.Length;
                return NumericType.Love;
            }

            index = -1;
            return NumericType.None;
        }


        /// <summary>
        /// Normalizes values from [-9, 9] range to [-100, 100] range
        /// </summary>
        /// <param name="numeric"></param>
        /// <returns></returns>
        public static int NormalizeNumeric(this int numeric)
        {
            if (numeric <= -9)
                return -100;
            if (numeric >= 9)
                return 100;

            return numeric * 10;
        }
    }
}