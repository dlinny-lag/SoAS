namespace AAF.Services.Reports
{
    public sealed class FileLexem : ReportLexem
    {
        public readonly string Path;
        public FileLexem(string path)
        {
            Path = path;
        }

        public override string Text => Path;
    }
}