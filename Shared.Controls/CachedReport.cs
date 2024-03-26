using System.Windows.Forms;
using AAF.Services.Reports;

namespace Shared.Controls
{
    public class CachedReport
    {
        private float fontSize;
        private string fontFamily;
        private string rtf;

        public CachedReport(Report report, DataGridViewCellStyle rtfStyle)
        {
            Report = report;
            fontSize = rtfStyle?.Font?.Size ?? -1f;
            fontFamily = rtfStyle?.Font?.FontFamily?.Name;
        }

        public bool ResetIfNecessary(DataGridViewCellStyle rtfStyle)
        {
            bool reset = rtfStyle?.Font?.Size != fontSize || rtfStyle?.Font?.FontFamily?.Name != fontFamily;
            if (!reset) 
                return false;

            rtf = null;
            fontSize = rtfStyle?.Font?.Size ?? -1f;
            fontFamily = rtfStyle?.Font?.FontFamily?.Name;
            return true;
        }

        public readonly Report Report;
        public string CachedText { get; private set; }

        public string CachedRtf => rtf ?? (rtf = Report.ToRtf((int)fontSize, fontFamily));


        public override string ToString()
        {
            return CachedText ?? (CachedText = Report.ToString());
        }
    }
}