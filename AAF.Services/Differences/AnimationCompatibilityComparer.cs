using System;
using System.Collections.Generic;
using AAF.Services.AAFImport;
using AAFModel;

namespace AAF.Services.Differences
{
    [Flags]
    public enum AnimationCompatibilityDifference
    {
        None = 0,
        ActorsSkeleton = 1,
        ActorsGender = 2,
        ActorsCount = 4,
    }
    public sealed class AnimationCompatibilityComparer : IElementComparer<Animation, AnimationCompatibilityDifference>
    {
        private readonly IDictionary<string, Defaults> defaults;
        private readonly IDictionary<string, Race> races;
        public AnimationCompatibilityComparer(IDictionary<string, Defaults> defaults, IDictionary<string, Race> races)
        {
            this.defaults = defaults;
            this.races = races;
        }
        public AnimationCompatibilityDifference Same(Animation a, Animation b)
        {
            if (a.Actors.Count != b.Actors.Count)
                return AnimationCompatibilityDifference.ActorsCount;

            AnimationCompatibilityDifference retVal = AnimationCompatibilityDifference.None;

            if (!defaults.TryGetValue(a.File, out var aDefaults))
                aDefaults = Defaults.Default;
            if (!defaults.TryGetValue(b.File, out var bDefaults))
                bDefaults = Defaults.Default;

            for (int i = 0; i < a.Actors.Count; i++)
            {
                var aActor = a.Actors[i];
                var bActor = b.Actors[i];
                if (aActor.GetSkeleton(aDefaults, races) != bActor.GetSkeleton(bDefaults, races))
                    retVal |= AnimationCompatibilityDifference.ActorsSkeleton;
                var aSex = aActor.Gender.ToSex();
                var bSex = bActor.Gender.ToSex();
                if (aSex != bSex && aSex != Sex.Any && bSex != Sex.Any)
                    retVal |= AnimationCompatibilityDifference.ActorsGender;
            }

            return retVal;
        }
    }
}