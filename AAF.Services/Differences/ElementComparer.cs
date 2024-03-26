using System;

namespace AAF.Services.Differences
{
    public interface IElementComparer
    {

    }

    public interface IElementComparer<in T, out TDiff> : IElementComparer
        where T: class
        where TDiff : Enum
    {
        TDiff Same(T a, T b);
    }
}