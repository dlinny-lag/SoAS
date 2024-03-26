using System;
using AAFModel;

namespace AAF.Services.Differences
{
    [Flags]
    public enum RaceDifference
    {
        None = 0,
        Skeleton = 1,
        FormId = 2,
    }
    public sealed class RaceComparer : IElementComparer<Race, RaceDifference>
    {
        public RaceDifference Same(Race a, Race b)
        {
            RaceDifference retVal = RaceDifference.None;

            if (a.Skeleton != b.Skeleton)
                retVal |= RaceDifference.Skeleton;
            if (a.FormId != b.FormId)
                retVal |= RaceDifference.FormId;
            return retVal;
        }
    }
}