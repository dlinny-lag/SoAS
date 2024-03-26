using AAF.Services.Differences;
using AAFModel;

namespace AAF.Services.Errors
{
    public sealed class PositionTreeDuplicationError : DuplicationError<PositionTree, PositionItemDifference>
    {
        private static readonly CollectionComparer<PositionTree, PositionItemDifference> comparer = 
            new CollectionComparer<PositionTree, PositionItemDifference>(PositionsTreeComparer.Default);
        public PositionTreeDuplicationError(string id, PositionTree reference) : base(id, reference)
        {
        }

        protected override CollectionComparer<PositionTree, PositionItemDifference> Comparer => comparer;
    }
}