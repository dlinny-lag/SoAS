using System.CodeDom.Compiler;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace JsonTreeView.Editors
{
    internal static class Validator
    {
        static readonly CodeDomProvider provider = CodeDomProvider.CreateProvider("C#");
        public static bool IsNameValid(this string value)
        {
            return provider.IsValidIdentifier(value);
        }

        public static JValue ToJValue(this string value)
        {
            if (value == JsonConvert.Null)
                return JValue.CreateNull();
            if (bool.TryParse(value, out bool boolVal))
                return new JValue(boolVal);
            if (int.TryParse(value, NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out int intVal))
                return new JValue(intVal);
            if (float.TryParse(value, NumberStyles.AllowLeadingSign|NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out float floatVal))
                return new JValue(floatVal);
            return new JValue(value);
        }
    }
}