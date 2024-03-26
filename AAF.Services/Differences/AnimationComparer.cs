using System;
using AAFModel;

namespace AAF.Services.Differences
{
    [Flags]
    public enum AnimationDifference
    {
        None = 0,
        Frames = 1,
        ActorsCount = 2,
        ActorsSkeleton = 4,
        ActorsGender = 8,
        ActorsIdle = 16,
    }
    public sealed class AnimationComparer : IElementComparer<Animation, AnimationDifference>
    {
        public static readonly AnimationComparer Default = new AnimationComparer();

        public AnimationDifference Same(Animation a, Animation b)
        {
            AnimationDifference retVal = AnimationDifference.None;
            if (a.Frames != b.Frames)
                retVal |= AnimationDifference.Frames;
            if (a.Actors.Count != b.Actors.Count)
                return retVal | AnimationDifference.ActorsCount;
            for (int i = 0; i < a.Actors.Count; i++)
            {
                var aActor = a.Actors[i];
                var bActor = b.Actors[i];
                if (aActor.Skeleton != bActor.Skeleton)
                    retVal |= AnimationDifference.ActorsSkeleton;
                if (aActor.Gender != bActor.Gender)
                    retVal |= AnimationDifference.ActorsGender;
                if (aActor.IdleFormId != bActor.IdleFormId)
                    retVal |= AnimationDifference.ActorsIdle;
                // TODO: race is insignificant
            }

            return retVal;
        }
    }
}