using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public sealed class AnimationsDuplicationDialog : DuplicationErrorsDialog<Animation, AnimationDifference, AnimationDuplicationError, AnimationsDuplicationErrorsList, AnimationsDuplicationDialog>
    {
        public AnimationsDuplicationDialog()
        {
            Text = "Animations duplications";
        }
    }
}