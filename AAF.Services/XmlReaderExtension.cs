using System;
using System.Collections.Generic;
using System.Globalization;
using System.Xml;
using Shared.Utils;

namespace AAFModel
{
    public static class XmlReaderExtension
    {
        public static bool Has(this IFileDescriptor fi, string pattern)
        {
            return fi.Name.IndexOf(pattern, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }

        public static bool EndsWith(this IFileDescriptor fi, string pattern)
        {
            return fi.Name.EndsWith(pattern, true, CultureInfo.InvariantCulture);
        }
        public static bool TryGetId(this XmlElement node, out string id)
        {
            id = null;
            foreach (XmlAttribute attr in node.Attributes)
            {
                if (attr.Name == "id")
                {
                    id = attr.Value;
                    return true;
                }
            }

            return false;
        }

        public static bool TryMap(this XmlAttribute attr, string name, Action<string> setter)
        {
            if (attr.Name == name)
            {
                setter(attr.Value);
                return true;
            }

            return false;
        }
        public static bool TryMap(this XmlAttribute attr, string name, Action<bool> setter)
        {
            if (attr.Name == name)
            {
                setter(string.Compare(attr.Value, "true", StringComparison.InvariantCultureIgnoreCase) == 0);
                return true;
            }

            return false;
        }

        public static bool TryMapInt(this XmlAttribute attr, string name, Action<int> setter)
        {
            if (attr.Name == name)
            {
                if (int.TryParse(attr.Value, NumberStyles.Integer, CultureInfo.InvariantCulture, out int value))
                {
                    setter(value);
                    return true;
                }
            }

            return false;
        }

        public static bool TryMapHex(this XmlAttribute attr, string name, Action<int> setter)
        {
            if (attr.Name == name)
            {
                if (int.TryParse(attr.Value, NumberStyles.HexNumber, CultureInfo.InvariantCulture, out int value))
                {
                    setter(value);
                    return true;
                }
            }

            return false;
        }

        public static IEnumerable<XmlElement> Elements(this XmlDocument doc, string elementName)
        {
            foreach (XmlNode root in doc)
            {
                foreach (XmlElement element in root)
                {
                    if (element.Name == elementName)
                        yield return element;
                }
            }
        }
    }
}