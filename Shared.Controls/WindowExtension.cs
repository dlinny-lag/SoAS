using System;
using System.Drawing;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Shared.Controls
{
    internal static class GDI
    {
        [StructLayout(LayoutKind.Sequential)]
        private class POINT
        {
            public int x;
            public int y;

            public POINT()
            {
            }

            public POINT(int x, int y)
            {
                this.x = x;
                this.y = y;
            }

            public Point ToPoint() => new Point(x, y);
        }

        [DllImport("gdi32.dll", EntryPoint = "OffsetViewportOrgEx", CharSet = CharSet.Auto, SetLastError = true)]
        private static extern bool IntOffsetViewportOrgEx(
            HandleRef hDC,
            int nXOffset,
            int nYOffset,
            [In, Out] POINT point);

        public static Point OffsetViewport(IntPtr hdc, int xOffset, int yOffset)
        {
            POINT point = new POINT();
            IntOffsetViewportOrgEx(new HandleRef(null, hdc), xOffset, yOffset, point);
            return point.ToPoint();
        }

        
        public const int SRCCOPY = 0x00CC0020;
        [DllImport("gdi32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        public static extern bool BitBlt(HandleRef hDC,
            int x,
            int y,
            int nWidth,
            int nHeight,
            HandleRef hSrcDC,
            int xSrc,
            int ySrc,
            int dwRop);
    }

    public static class WindowExtension
    {
        private static readonly MethodInfo setStyle;
        private static readonly MethodInfo getStyle;
        static WindowExtension()
        {
            setStyle = typeof(Control).GetMethod("SetStyle", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
            getStyle = typeof(Control).GetMethod("GetStyle", BindingFlags.InvokeMethod | BindingFlags.NonPublic | BindingFlags.FlattenHierarchy | BindingFlags.Instance);
        }

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        public static extern IntPtr SendMessage(HandleRef hWnd, int msg, IntPtr wParam, IntPtr lParam);

        public static void UpdateStyle(this Control control, ControlStyles styles, bool value)
        {

            setStyle.Invoke(control, new object[] { styles, value });
        }

        public static bool GetStyle(this Control control, ControlStyles styles)
        {
            return (bool)getStyle.Invoke(control, new object[] { styles });
        }

        public const int WM_PRINT = 791;
        public const int WM_PAINT = 15;
        public const int WM_MOUSEWHEEL = 0x20A;


        public const int PRF_CHECKVISIBLE = 0x00000001;
        public const int PRF_NONCLIENT = 0x00000002;
        public const int PRF_CLIENT = 0x00000004;
        public const int PRF_ERASEBKGND = 0x00000008;
        public const int PRF_CHILDREN = 0x00000010;
        public const int PRF_OWNED = 0x00000020;


        public static IntPtr SendMessage(this Control control, int msg, IntPtr wParam, IntPtr lParam)
        {
            // TODO: ensure handle created
            return SendMessage(new HandleRef((object) control, control.Handle), msg, wParam, lParam);
        }
    }

}