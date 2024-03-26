using System;
using AAFModel;

namespace AAF.Services.Reports
{
    public enum ReferenceLexemTypes
    {
        None,
        Position,
        Animation,
        AnimationGroup,
        PositionTree
    }

    public sealed class ReferenceLexem : ReportLexem
    {
        public readonly string Id;
        public readonly ReferenceLexemTypes Type;
        public readonly string Link;
        private ReferenceLexem(string id, ReferenceType type, string link)
        {
            Id = id;
            Type = (ReferenceLexemTypes)(type+1);
            Link = link;
        }

        private ReferenceLexem(string positionId, string link)
        {
            Id = positionId;
            Type = ReferenceLexemTypes.Position;
            Link = link;
        }

        private ReferenceLexem(string id)
        {
            Id = id;
            Type = ReferenceLexemTypes.None;
        }

        public static ReferenceLexem Ref(string id, ReferenceType type, string link = null)
        {
            if (type == ReferenceType.None)
                throw new ArgumentOutOfRangeException(nameof(type));
            if (type > ReferenceType.PositionTree)
                throw new ArgumentOutOfRangeException(nameof(type));
            return new ReferenceLexem(id, type, link);
        }
            
        public static ReferenceLexem Anim(string animationId, string link = null) =>
            new ReferenceLexem(animationId, ReferenceType.Animation, link);
        public static ReferenceLexem AnimGroup(string animationGroupId, string link = null) =>
            new ReferenceLexem(animationGroupId, ReferenceType.AnimationGroup, link);
        public static ReferenceLexem Tree(string treeId, string link = null) =>
            new ReferenceLexem(treeId, ReferenceType.PositionTree, link);
        public static ReferenceLexem Pos(string positionId, string link = null) =>
            new ReferenceLexem(positionId, link);
        public static ReferenceLexem Miss(string id) => new ReferenceLexem(id);


        public override string Text => Id;
    }
}