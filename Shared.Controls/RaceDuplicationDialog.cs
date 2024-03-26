using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public sealed class RaceDuplicationDialog : DuplicationErrorsDialog<Race, RaceDifference, RaceDuplicationError, RaceDuplicationErrorsList, RaceDuplicationDialog>
    {
        public RaceDuplicationDialog()
        {
            Text = "Race duplications";
        }
    }
}