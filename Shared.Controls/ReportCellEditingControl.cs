using System;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    public class ReportCellEditingControl : RichTextBox , IDataGridViewEditingControl
    {
        private DataGridViewCellStyle style;
        public void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle)
        {
            style = dataGridViewCellStyle;
            // emulate semitransparent. alpha = 0.5

            BackColor = Semi(dataGridViewCellStyle.SelectionBackColor);
            WordWrap = dataGridViewCellStyle.WrapMode == DataGridViewTriState.True;
        }

        private const double alpha = 0.5;
        static Color Semi(Color color)
        {
            return Color.FromArgb(Blend(color.R), Blend(color.G), Blend(color.B));
        }

        static int Blend(byte c)
        {
            int b = (int)((byte.MaxValue - c) * alpha + c);
            if (b < 0)
                b = 0;
            if (b > byte.MaxValue)
                b = byte.MaxValue;
            return b;
        }

        public virtual bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey)
        {
            if (keyData == Keys.Escape)
            {
                EditingControlDataGridView.EndEdit();
                return false;
            }

            return true;
        }

        public object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context)
        {
            return "";
        }

        public void PrepareEditingControlForEdit(bool selectAll)
        {
            ReadOnly = true;
            BorderStyle = BorderStyle.None;
            Multiline = true;
            ScrollBars = RichTextBoxScrollBars.None;

            var cell = EditingControlDataGridView.CurrentCell as DataGridViewReportCell;
            if (cell == null)
                return;
            var report = cell.GetReport(EditingControlRowIndex);
            if (report == null)
                return;
            report.ResetIfNecessary(style);

            var margins = cell.GetBordersWidth();
            
            Width = Parent.Width - cell.Padding.Horizontal;
            Left = cell.Padding.Left - margins.Left;
            Top = cell.Padding.Top;
            
            Rtf = report.CachedRtf;
        }

        public DataGridView EditingControlDataGridView { get; set; }
        public object EditingControlFormattedValue { get; set; }
        public int EditingControlRowIndex { get; set; }
        public bool EditingControlValueChanged { get; set; } = false;
        public Cursor EditingPanelCursor { get; } = Cursors.IBeam;
        public bool RepositionEditingControlOnValueChange { get; } = false;
    }
}