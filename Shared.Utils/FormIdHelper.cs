namespace Shared.Utils
{
    public static class FormIdHelper
    {
        public static int NormalizeFormId(this int formId)
        {
            const uint eslMark = 0xFE000000;
            const uint eslMask = 0x00000FFF;
            const uint mask = 0x00FFFFFF;

            bool isEsl = (formId & eslMark) == eslMark;

            if (isEsl)
                return (int)(formId & eslMask);
            return (int)(formId & mask);
        }
    }
}