using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public sealed class PositionTreeDuplicationDialog : DuplicationErrorsDialog<PositionTree, PositionItemDifference, PositionTreeDuplicationError, PositionsTreeDuplicationErrorsList, PositionTreeDuplicationDialog>
    {
        public PositionTreeDuplicationDialog()
        {
            Text = "Positions tree duplications";
        }
    }
}