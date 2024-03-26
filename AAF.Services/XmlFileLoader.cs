using System;
using System.IO;
using System.Reflection;
using System.Xml;
using AAFModel;

namespace AAF.Services
{
    static class XmlFileLoader
    {
        static readonly XmlReaderSettings settings = new XmlReaderSettings { IgnoreComments = true };

        public static XmlDocument LoadString(string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return null;

            TextReader tReader = new StringReader(content);
            XmlReader reader = XmlReader.Create(tReader, settings);
            XmlDocument doc = new XmlDocument();
            try
            {
                doc.Load(reader);
                return doc;
            }
            catch (XmlException ex)
            {
                if (ex.IsMultipleRoots())
                {
                    return LoadString($"<{FakeRoot}>{content}</{FakeRoot}>");
                }
                if (ex.IsWrongComment())
                {
                    return LoadString(FixComments(content));
                }

                if (ex.IsWrongAmpersand1())
                {
                    return LoadString(FixAmpersand(content));
                }

                if (ex.IsWrongAmpersand2(content))
                {
                    return LoadString(FixAmpersand(content));
                }
                throw new LoadException(content, ex);
            }
            catch (Exception e)
            {
                throw new LoadException(content, e);
            }
        }

#region errors handling

        private const string FakeRoot = "fake_root";
        private const string triplet = "---";
        /// <summary>
        ///  Several XML files are badly formatted.
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        static string FixComments(string content)
        {
            int index = 1;
            do
            {
                index = content.IndexOf(triplet, index, StringComparison.InvariantCultureIgnoreCase);
                if (index > 0)
                {
                    int ending = index + triplet.Length;
                    for (int i = ending; i < content.Length; i++)
                    {
                        if (content[i] != '-')
                        {
                            ending = i;
                            break;
                        }
                    }

                    content = content.Substring(0, index) + "--" + content.Substring(ending);
                }

                index++;
            } while (index > 0 && index < content.Length);

            return content;
        }

        const string XmlAmpersand = "&amp;";
        static string FixAmpersand(string content)
        {
            int index = 1;
            do
            {
                index = content.IndexOf('&', index);
                if (index > 0)
                {
                    if (content.Substring(index, XmlAmpersand.Length) != XmlAmpersand)
                    {
                        content = content.Substring(0, index) + XmlAmpersand + content.Substring(index + 1);
                    }

                    index++;
                }
            } while (index > 0 && index < content.Length);

            return content;
        }

        static readonly MethodInfo resStringGetter = 
            typeof(XmlException)
                .GetProperty("ResString", BindingFlags.DeclaredOnly|BindingFlags.NonPublic|BindingFlags.Instance)
                .GetMethod;

        private static readonly FieldInfo argsField =
            typeof(XmlException).GetField("args", BindingFlags.DeclaredOnly | BindingFlags.NonPublic | BindingFlags.Instance);

        private static readonly string MultipleRootsError = "Xml_MultipleRoots";
        private static readonly string WrongAmpersandError = "Xml_UnexpectedTokenEx";
        private static readonly string WrongCommentError = "Xml_InvalidCommentChars";
        private static readonly string ErrorParsingEntityName = "Xml_ErrorParsingEntityName"; 
        private static bool IsMultipleRoots(this XmlException ex)
        {
            return MultipleRootsError == resStringGetter.Invoke(ex, null) as string;
        }

        private static bool IsWrongAmpersand1(this XmlException ex)
        {
            string resString = resStringGetter.Invoke(ex, null) as string;

            if (WrongAmpersandError != resString)
                return false;

            string[] args = argsField.GetValue(ex) as string[];
            if (args == null || args.Length < 2)
                return false;
            return args[1] == ";"; // finishing symbol of &amp; is expected, but not found. may happen when & symbol was not escaped
        }

        private static bool IsWrongAmpersand2(this XmlException ex, string content)
        {
            string resString = resStringGetter.Invoke(ex, null) as string;

            if (ErrorParsingEntityName != resString)
                return false;
            string errorLine = null;
            using (var linesReader = new StringReader(content))
            {
                for (int i = 0; i < ex.LineNumber; i++)
                    errorLine = linesReader.ReadLine();
            }

            if (string.IsNullOrWhiteSpace(errorLine))
                return false; // wrong line detection?
            int index = errorLine.IndexOf('&', 0);
            if (index < 0)
                return false; // ampersand is not the reason of error
            if (index > ex.LinePosition)
                return false; // ampersand exist, but it might not be a reason of error
            return content.Substring(index, XmlAmpersand.Length) != XmlAmpersand; // TODO: maybe it is not the actual reason, but it become too complicated
        }

        private static bool IsWrongComment(this XmlException ex)
        {
            return WrongCommentError == resStringGetter.Invoke(ex, null) as string;
        }
#endregion
    }
}
