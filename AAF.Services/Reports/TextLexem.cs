namespace AAF.Services.Reports
{
    public enum TextType
    {
        None, // default
        Highlight, // to highlight something
        Important, // important, need to pay attention
    }

    public sealed class TextLexem : ReportLexem
    {
        public override string Text { get; }

        public readonly TextType Type;

        public TextLexem(string text, TextType type)
        {
            Text = text;
            Type = type;
        }

        public static readonly TextLexem NewLine = new TextLexem("\n", TextType.None);
    }
}