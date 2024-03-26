using System.Collections.Generic;

namespace AAFModel
{
    public sealed class PositionItem
    {
        public string PositionId { get; set; }
        public string BranchId { get; set; }
        public List<PositionItem> Children { get; set; }
    }

    public sealed class PositionTree : Referenceable
    {
        public PositionItem Root { get; set; }

        public PositionTree(string file, int order) : base(file, order)
        {
        }

        public override ReferenceType Type => ReferenceType.PositionTree;
    }
}