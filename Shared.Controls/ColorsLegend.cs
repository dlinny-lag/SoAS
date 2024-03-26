using System.Windows.Forms;

namespace Shared.Controls
{
    public partial class ColorsLegend : UserControl
    {
        public ColorsLegend()
        {
            InitializeComponent();
            animationLabel.ForeColor = ReportConverter.AnimationColor.ToDrawingColor();
            positionLabel.ForeColor = ReportConverter.PositionColor.ToDrawingColor();
            groupLabel.ForeColor = ReportConverter.GroupColor.ToDrawingColor();
            treeLable.ForeColor = ReportConverter.TreeColor.ToDrawingColor();
        }
    }
}
