using AAF.Services.Differences;
using AAF.Services.Reports;
using AAFModel;

namespace AAF.Services.Errors
{
    public abstract class BadPositionsTreeError : PositionError
    {
        protected BadPositionsTreeError(string positionId, string treeId) : base(positionId)
        {
            TreeId = treeId;
        }

        public readonly string TreeId;
    }

    public sealed class PositionsCircleError : BadPositionsTreeError
    {
        private readonly PositionItem wrongItem;
        public PositionsCircleError(string positionId, string treeId, PositionItem item) : base(positionId, treeId)
        {
            wrongItem = item;
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ").AppendTree(TreeId)
                .AppendNotice(" that has a branch ").AppendCritical(wrongItem.BranchId)
                .AppendNotice(" that points to ").AppendPos(wrongItem.PositionId)
                .AppendNotice(" that points back to the PositionsTree");

            return retVal;
        }
    }

    public sealed class TreeHasPositionWithMissingReference : BadPositionsTreeError
    {
        public TreeHasPositionWithMissingReference(string positionId, string treeId, string badPosition) : base(positionId, treeId)
        {
            PositionWithMissingReference = badPosition;
        }

        public readonly string PositionWithMissingReference;
        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ").AppendTree(TreeId)
                .AppendNotice(" that points to position ").AppendPos(PositionWithMissingReference)
                .AppendNotice(" with missing reference");
            return retVal;
        }
    }

    public sealed class TreeIncompatibleAnimationsError : BadPositionsTreeError
    {
        private readonly AnimationCompatibilityDifference diff;
        public TreeIncompatibleAnimationsError(string positionId, string treeId, AnimationCompatibilityDifference diff) : base(positionId, treeId)
        {
            this.diff = diff;
        }

        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ").AppendTree(TreeId)
                .AppendNotice(" that has animations incompatible in: ").AppendCritical($"{diff}");

            return retVal;
        }
    }

    public sealed class TreeHasMissingPositionError : BadPositionsTreeError
    {
        public TreeHasMissingPositionError(string positionId, string treeId, string missingPosition) : base(positionId, treeId)
        {
            MissingPosition = missingPosition;
        }

        public readonly string MissingPosition;
        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ").AppendTree(TreeId)
                .AppendNotice(" that points to missing ").AppendPos(MissingPosition);
            return retVal;
        }
    }

    public sealed class TreeHasInvalidPositionError : BadPositionsTreeError
    {
        public TreeHasInvalidPositionError(string positionId, string treeId, string invalidPositionId, ReferenceType refType) : base(positionId, treeId)
        {
            InvalidPositionId = invalidPositionId;
            Type = refType;
        }

        public readonly string InvalidPositionId;
        public readonly ReferenceType Type;
        public override Report Report()
        {
            Report retVal = new Report();
            retVal.AppendPos(PositionId).AppendNotice(" points to ").AppendTree(TreeId)
                .AppendNotice(" that points to ").AppendPos(InvalidPositionId)
                .Append(" that points to ")
                .AppendCritical(Type.ToString())
                .AppendNotice("  instead of Animation");
            return retVal;
        }
    }
}