using System;
using AAFModel;

namespace AAF.Services.Differences
{
    [Flags]
    public enum AnimationGroupDifference
    {
        None = 0,
        Animations = 1,
    }

    public sealed class AnimationGroupComparer : IElementComparer<AnimationGroup, AnimationGroupDifference>
    {
        public static readonly AnimationGroupComparer Default = new AnimationGroupComparer();

        private static readonly CollectionComparer<string, StringDifference> comparer = new CollectionComparer<string, StringDifference>(StringsComparer.Default);
        public AnimationGroupDifference Same(AnimationGroup a, AnimationGroup b)
        {
            if (comparer.AreSame(a.Animations, b.Animations))
                return AnimationGroupDifference.Animations;
            return AnimationGroupDifference.None;
        }
    }
}