using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public sealed class PositionDuplicationDialog : DuplicationErrorsDialog<Position, PositionDifference, PositionDuplicationError, PositionDuplicationErrorsList, PositionDuplicationDialog>
    {
        public PositionDuplicationDialog()
        {
            Text = "Positions duplications";
        }
    }
}