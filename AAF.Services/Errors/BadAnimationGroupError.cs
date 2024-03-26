using System.Collections.Generic;
using AAF.Services.Differences;
using AAF.Services.Reports;

namespace AAF.Services.Errors
{
    public abstract class BadAnimationGroupError : PositionError
    {
        protected BadAnimationGroupError(string positionId, string groupId) : base(positionId)
        {
            GroupId = groupId;
        }

        public readonly string GroupId;
    }

    public sealed class GroupMissingAnimation : BadAnimationGroupError
    {
        private readonly IList<string> missingAnims;
        public GroupMissingAnimation(string positionId, string groupId, IList<string> missingAnims) : base(positionId, groupId)
        {
            this.missingAnims = missingAnims;
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ")
                .AppendGroup(GroupId).AppendNotice(" that has missing Animations: ");
            for (int i = 0; i < missingAnims.Count; i++)
            {
                if (i > 0)
                    retVal.AppendNotice(", ");
                retVal.AppendAnim(missingAnims[i]);
            }

            return retVal;
        }
    }

    public sealed class GroupIncompatibleAnimationsError : BadAnimationGroupError
    {
        private readonly AnimationCompatibilityDifference diff;
        public GroupIncompatibleAnimationsError(string positionId, string groupId, AnimationCompatibilityDifference diff) : base(positionId, groupId)
        {
            this.diff = diff;
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ").AppendGroup(GroupId)
                .AppendNotice($" that has animations incompatible in: ").AppendCritical($"{diff}");

            return retVal;
        }
    }
}