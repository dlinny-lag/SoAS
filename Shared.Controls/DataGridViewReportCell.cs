using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Windows.Forms;

namespace Shared.Controls
{
    public sealed class DataGridViewReportCell : DataGridViewCell
    {
        private static readonly Dictionary<Color, SolidBrush> cachedBrushes = new Dictionary<Color, SolidBrush>(2);
        
        private static RichTextBox _reference; // TODO: should be single instance per each DataGridView simultaneously displayed
        private RichTextBox Reference
        {
            get
            {
                if (_reference != null)
                    return _reference;
                
                _reference = new RichTextBox() { BorderStyle = BorderStyle.None, Multiline = true, ScrollBars = RichTextBoxScrollBars.None };
                _reference.ContentsResized += OnContentResized;
                return _reference;
            }
        }

        static void OnContentResized(object sender, ContentsResizedEventArgs e)
        {
            _reference.Height = e.NewRectangle.Height;
        }

        public override Type ValueType
        {
            get => typeof(CachedReport);
            set { }
        }

        public override Type FormattedValueType => typeof(string);
        public override Type EditType { get; } = typeof(ReportCellEditingControl);
        public Padding Padding { get; set; } = new Padding(1);

        public override object Clone()
        {
            var retVal = (DataGridViewReportCell)base.Clone();
            retVal.Padding = Padding;
            return retVal;
        }

        public Rectangle GetBordersWidth()
        {
            // DataGridViewRow.PaintCells
            DataGridViewAdvancedBorderStyle advancedBorderStyle = 
                AdjustCellBorderStyle(DataGridView.AdvancedCellBorderStyle,
                    new DataGridViewAdvancedBorderStyle(), 
                    DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single || DataGridView.CellBorderStyle == DataGridViewCellBorderStyle.SingleVertical, 
                    DataGridView.AdvancedCellBorderStyle.All == DataGridViewAdvancedCellBorderStyle.Single || DataGridView.CellBorderStyle == DataGridViewCellBorderStyle.SingleHorizontal, 
                    ColumnIndex == 0, RowIndex == 0);

            return BorderWidths(advancedBorderStyle);
        }

        public CachedReport GetReport(int rowIndex)
        {
            return GetValue(rowIndex) as CachedReport;
        }

        protected override Size GetPreferredSize(Graphics graphics, DataGridViewCellStyle cellStyle, int rowIndex, Size constraintSize)
        {
            if (constraintSize.Width == 0)
                return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
            var report = GetValue(rowIndex) as CachedReport;
            if (report == null)
                return base.GetPreferredSize(graphics, cellStyle, rowIndex, constraintSize);
            
            report.ResetIfNecessary(cellStyle);
            Reference.WordWrap = cellStyle.WrapMode == DataGridViewTriState.True;
            var borders = GetBordersWidth();
            int newWidth = constraintSize.Width - Padding.Horizontal - borders.Width;
            Reference.Width = newWidth;
            if (Reference.Width != newWidth)
            { // for unknown reason sometimes Width is not changed. Try again, seems working work around.
                Reference.Width = newWidth;
            }

            string rtf = report.CachedRtf;
            Reference.Rtf = rtf;
            return Reference.Size + Padding.Size;
        }

        protected override void Paint(Graphics graphics, Rectangle clipBounds, Rectangle cellBounds, int rowIndex,
            DataGridViewElementStates cellState, object value, object formattedValue, string errorText,
            DataGridViewCellStyle cellStyle, DataGridViewAdvancedBorderStyle advancedBorderStyle,
            DataGridViewPaintParts paintParts)
        {
            bool selected = (cellState & DataGridViewElementStates.Selected) > 0;
            Color backColor = selected ? cellStyle.SelectionBackColor : cellStyle.BackColor;
            PaintBorder(graphics, clipBounds, cellBounds, cellStyle, advancedBorderStyle);
            if (!cachedBrushes.TryGetValue(backColor, out var brush))
            {
                brush = new SolidBrush(backColor);
                cachedBrushes.Add(backColor, brush);
            }

            Rectangle margins = BorderWidths(advancedBorderStyle);
            Rectangle toFill = cellBounds;
            toFill.Offset(margins.X, margins.Y);
            toFill.Width -= margins.Right;
            toFill.Height -= margins.Bottom;
            graphics.FillRectangle(brush, toFill);

            var report = value as CachedReport;
            if (report == null)
                return;

            report.ResetIfNecessary(cellStyle);
            Reference.Rtf = report.CachedRtf;
            Reference.BackColor = backColor;
            DrawText(graphics, ref cellBounds);
        }


        private static readonly IntPtr PrintLParam =
            new IntPtr( WindowExtension.PRF_ERASEBKGND |
                        WindowExtension.PRF_CHILDREN |
                        WindowExtension.PRF_CLIENT |
                        WindowExtension.PRF_NONCLIENT |
                        WindowExtension.PRF_OWNED);
        private void DrawText(Graphics graphics, ref Rectangle cellBounds)
        {
            int xOffset = cellBounds.Left + Padding.Left;
            int yOffset = cellBounds.Top + Padding.Top;

            var hdc = graphics.GetHdc();
            GDI.OffsetViewport(hdc, xOffset, yOffset);
            try
            {
                Reference.SendMessage(WindowExtension.WM_PRINT, hdc, PrintLParam);
            }
            finally
            {
                GDI.OffsetViewport(hdc, -xOffset, -yOffset);
                graphics.ReleaseHdc(hdc);
            }
        }
    }
}