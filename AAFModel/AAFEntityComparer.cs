using System.Collections.Generic;

namespace AAFModel
{
    public sealed class AAFEntityComparer<T> : IComparer<T>
        where T:Declared
    {
        private readonly IDictionary<string, Defaults> defaults;
        public AAFEntityComparer(IDictionary<string, Defaults> defaults)
        {
            this.defaults = defaults;
        }

        public int Compare(T x, T y)
        {
            if (x.File == y.File)
                return x.Order.CompareTo(y.Order);

            int xPriority = defaults[x.File].Priority;
            int yPriority = defaults[y.File].Priority;

            int byPriority = yPriority.CompareTo(xPriority); // see AAF.swf: DataSetBase.isHighestPriority
            if (byPriority != 0)
                return byPriority;

            // TODO: potential desync with AAF
            int byId = x.Id.CompareTo(y.Id);
            return byId;
        }
    }
}