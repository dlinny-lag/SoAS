using System;
using AAF.Services.Reports;
using RTFExporter;

namespace Shared.Controls
{
    public static class ReportConverter
    {
        public static readonly Color FileColor = new Color(64, 64, 64);
        public static readonly Color TextColor = Color.black;
        public static readonly Color HighlightedTextColor = new Color(255, 70, 0); // orange
        public static readonly Color PositionColor = Color.blue;
        public static readonly Color AnimationColor = Color.green;
        public static readonly Color GroupColor = Color.cyan;
        public static readonly Color TreeColor = Color.purple;
        public static readonly Color MissingTypeColor = Color.red;

        public static System.Drawing.Color ToDrawingColor(this Color color)
        {
            return System.Drawing.Color.FromArgb(color.r, color.g, color.b);
        }

        private static RTFTextStyle TextStyle(int fontSize, string fontFamily, TextType type)
        {
            Color color;
            switch (type)
            {
                case TextType.Highlight:
                    color = HighlightedTextColor;
                break;
                case TextType.Important:
                    color = Color.red;
                break;
                default:
                    color = TextColor;
                break;
            }
            return new RTFTextStyle(false, false, false, false, false, false, fontSize, fontFamily, color, Underline.None);
        }

        private static RTFTextStyle FileStyle(int fontSize, string fontFamily)
        {
            return new RTFTextStyle(false, false, false, false, false, false, fontSize, fontFamily, FileColor, Underline.Basic);
        }

        private static RTFTextStyle RefStyle(this ReferenceLexem reference, int fontSize, string fontFamily)
        {
            Color color;
            switch (reference.Type)
            {
                case ReferenceLexemTypes.None:
                    color = MissingTypeColor;
                    break;
                case ReferenceLexemTypes.Position:
                    color = PositionColor;
                    break;
                case ReferenceLexemTypes.Animation:
                    color = AnimationColor;
                    break;
                case ReferenceLexemTypes.AnimationGroup:
                    color = GroupColor;
                    break;
                case ReferenceLexemTypes.PositionTree:
                    color = TreeColor;
                    break;
                default:
                    throw new InvalidOperationException("Unknown reference type");
            }
            return new RTFTextStyle(false, false, false, false, false, false, fontSize, fontFamily, color, Underline.None);
        }
        public static string ToRtf(this Report report, int fontSize = 16, string fontFamily = "Arial")
        {
            var lexems = report.Lexems;
            if (lexems.Count == 0)
                return "";

            RTFDocument doc = new RTFDocument();
            RTFParagraph paragraph = new RTFParagraph(doc);
            paragraph.style = new RTFParagraphStyle(Alignment.Left, new Indent(0, 0, 0), 0, 0);

            for (int i = 0; i < lexems.Count; i++)
            {
                ReportLexem lexem = lexems[i];
                switch (lexem)
                {
                    case FileLexem asFile:
                        paragraph.AppendText(asFile.Path.GetRtfString(), FileStyle(fontSize, fontFamily));
                        break;
                    case TextLexem asText:
                        paragraph.AppendText(asText.Text.GetRtfString(), TextStyle(fontSize, fontFamily, asText.Type));
                        break;
                    case ReferenceLexem asRef:
                        paragraph.AppendText(asRef.Text, RefStyle(asRef, fontSize, fontFamily));
                        break;
                    default:
                        throw new InvalidOperationException("Unsupported lexem type");
                }
            }
            return RTFParser.ToString(doc);
        }
        
    }
}