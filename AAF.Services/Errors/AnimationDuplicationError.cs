using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class AnimationDuplicationError : DuplicationError<Animation, AnimationDifference>
    {
        private static readonly CollectionComparer<Animation, AnimationDifference> animationsComparer =
            new CollectionComparer<Animation, AnimationDifference>(AnimationComparer.Default);

        public AnimationDuplicationError(string animationId, Animation reference) : base(animationId, reference)
        {
        }

        protected override CollectionComparer<Animation, AnimationDifference> Comparer => animationsComparer;
    }
}