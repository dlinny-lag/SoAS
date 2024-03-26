using System;
using AAFModel;

namespace AAF.Services.Differences
{
    [Flags]
    public enum PositionItemDifference
    {
        None,
        PositionId = 1,
        Children = 2,
    }

    sealed class PositionItemComparer : IElementComparer<PositionItem, PositionItemDifference>
    {

        private static readonly CollectionComparer<PositionItem, PositionItemDifference> itemsComparer =
            new CollectionComparer<PositionItem, PositionItemDifference>(new PositionItemComparer());
        public PositionItemDifference Same(PositionItem a, PositionItem b)
        {
            PositionItemDifference retVal = PositionItemDifference.None;
            if (a.PositionId != b.PositionId)
                retVal |= PositionItemDifference.PositionId;
            if (!itemsComparer.AreSame(a.Children, b.Children))
                retVal |= PositionItemDifference.Children;
            return retVal;
        }
    }

    public sealed class PositionsTreeComparer : IElementComparer<PositionTree, PositionItemDifference>
    {
        public static readonly PositionsTreeComparer Default = new PositionsTreeComparer();

        private static readonly PositionItemComparer comparer = new PositionItemComparer();
        public PositionItemDifference Same(PositionTree a, PositionTree b)
        {
            return comparer.Same(a.Root, b.Root);
        }
    }
}