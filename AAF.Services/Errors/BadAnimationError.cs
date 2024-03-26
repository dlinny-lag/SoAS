using AAF.Services.Reports;

namespace AAF.Services.Errors
{
    public abstract class BadAnimationError : PositionError
    {
        protected BadAnimationError(string positionId, string animationId) : base(positionId)
        {
            AnimationId = animationId;
        }

        public readonly string AnimationId;
    }

    public sealed class SkeletonUndefinedError : BadAnimationError
    {
        public SkeletonUndefinedError(string positionId, string animationId) : base(positionId, animationId)
        {
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId)
                .AppendNotice(" points to ")
                .AppendAnim(AnimationId)
                .AppendNotice(" that has an actor with unresolved skeleton");
            return retVal;
        }
    }
}