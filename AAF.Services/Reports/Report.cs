using System.Collections.Generic;
using System.Text;

namespace AAF.Services.Reports
{
    public sealed class Report
    {
        private List<ReportLexem> lexems = new List<ReportLexem>(5);

        public Report Append(ReportLexem lexem)
        {
            lexems.Add(lexem);
            return this;
        }

        public Report AppendLine(ReportLexem lexem)
        {
            lexems.Add(lexem);
            lexems.Add(TextLexem.NewLine);
            return this;
        }

        public IList<ReportLexem> Lexems => lexems.ToArray();

        public override string ToString()
        {
            if (lexems.Count == 0)
                return "";
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < lexems.Count; i++)
            {
                sb.Append(lexems[i].Text);
            }

            return sb.ToString();
        }
    }
}