using AAFModel;

namespace AAF.Services.Reports
{
    public static class ReportExtension
    {
        public static Report Append(this Report report, string text, TextType type = TextType.None)
        {
            return report.Append(new TextLexem(text, type));
        }
        public static Report AppendNotice(this Report report, string text) => report.Append(text, TextType.Highlight);
        public static Report AppendCritical(this Report report, string text) => report.Append(text, TextType.Important);

        public static Report AppendFile(this Report report, string path)
        {
            return report.Append(new FileLexem(path));
        }

        public static Report AppendPos(this Report report, string positionId, string link = null)
        {
            return report.Append(ReferenceLexem.Pos(positionId, link));
        }

        public static Report AppendAnim(this Report report, string animationId, string link = null)
        {
            return report.Append(ReferenceLexem.Anim(animationId, link));
        }

        public static Report AppendGroup(this Report report, string groupId, string link = null)
        {
            return report.Append(ReferenceLexem.AnimGroup(groupId, link));
        }

        public static Report AppendTree(this Report report, string treeId, string link = null)
        {
            return report.Append(ReferenceLexem.Tree(treeId, link));
        }

        public static Report AppendRef(this Report report, string id, ReferenceType type, string link = null)
        {
            return report.Append(ReferenceLexem.Ref(id, type, link));
        }
    }
}