using System.Text;

namespace Shared.Utils
{
    public static class StringUtils
    {
        public static StringBuilder Tab(this StringBuilder sb, uint count = 1)
        {
            for (int i = 0; i < count; i++)
                sb.Append('\t');
            return sb;
        }
        public static StringBuilder Space(this StringBuilder sb, uint count = 1)
        {
            for (int i = 0; i < count; i++)
                sb.Append(' ');
            return sb;
        }
    }
}