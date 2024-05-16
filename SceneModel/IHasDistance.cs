using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Text;

namespace SceneModel
{
    public interface IHasDistance<in T>
        where T : IHasDistance<T>
    {
        ulong Distance(T other);
    }

    public static class DistanceHelper
    {
        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Diff(this int a, int b)
        {
            return (ulong)Math.Abs(a - b);
        }

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Diff<T>(this T a, T b)
            where T : Enum
        {
            return Diff(Convert.ToInt32(a), Convert.ToInt32(b));  // TODO: handle other types of underlying type
        }

        [Pure, MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ulong Diff(this string a, string b)
        {
            byte[] aData = a == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(a);
            byte[] bData = b == null ? Array.Empty<byte>() : Encoding.UTF8.GetBytes(b);

            int lenA = aData.Length;
            int lenB = bData.Length;
            int min = Math.Min(lenA, lenB);
            ulong retVal = 0;
            for (int i = 0; i < min; i++)
            {
                retVal += (ulong)Math.Abs((int)aData[i] - (int)bData[i]);
            }

            if (lenA > min)
                retVal += Weight(bData, min);
            if (lenB > min)
                retVal += Weight(aData, min);

            return retVal;
        }

        private static ulong Weight(this byte[] toCompare, int startIndex)
        {
            ulong retVal = 0;
            for (int i = startIndex; i < toCompare.Length; i++)
            {
                retVal += toCompare[i];
            }

            return retVal;
        }
    }
}