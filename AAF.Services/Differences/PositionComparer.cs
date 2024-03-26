using System;
using AAFModel;

namespace AAF.Services.Differences
{
    [Flags]
    public enum PositionDifference
    {
        None = 0,
        Reference = 1,
        ReferenceType = 2,
        Hidden = 4,
    }

    public sealed class PositionComparer : IElementComparer<Position, PositionDifference>
    {
        public static PositionComparer Default = new PositionComparer();

        public PositionDifference Same(Position a, Position b)
        {
            // locations and tags are not significant and should be merged

            PositionDifference retVal = PositionDifference.None;

            if (a.Reference != b.Reference)
                retVal |= PositionDifference.Reference;
            if (a.ReferenceType != b.ReferenceType)
                retVal |= PositionDifference.ReferenceType; 
            if (a.IsHidden != b.IsHidden)
                retVal |= PositionDifference.Hidden;

            return retVal;
        }
    }
}