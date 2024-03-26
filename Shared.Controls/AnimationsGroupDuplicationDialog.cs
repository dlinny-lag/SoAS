using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAF.Services.Errors;
using AAFModel;

namespace Shared.Controls
{
    public sealed class AnimationsGroupDuplicationDialog : DuplicationErrorsDialog<AnimationGroup, AnimationGroupDifference, AnimationGroupDuplicationError, AnimationsGroupsDuplicationErrorsList, AnimationsGroupDuplicationDialog>
    {
        public AnimationsGroupDuplicationDialog()
        {
            Text = "Animations groups duplications";
        }
    }
}