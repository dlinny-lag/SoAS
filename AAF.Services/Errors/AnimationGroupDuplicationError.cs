using AAF.Services.AAFImport;
using AAF.Services.Differences;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class AnimationGroupDuplicationError : DuplicationError<AnimationGroup, AnimationGroupDifference>
    {
        private static readonly CollectionComparer<AnimationGroup, AnimationGroupDifference> comparer =
            new CollectionComparer<AnimationGroup, AnimationGroupDifference>(AnimationGroupComparer.Default);
        public AnimationGroupDuplicationError(string animationGroupId, AnimationGroup reference) : base(animationGroupId, reference)
        {
        }

        protected override CollectionComparer<AnimationGroup, AnimationGroupDifference> Comparer => comparer;
    }
}