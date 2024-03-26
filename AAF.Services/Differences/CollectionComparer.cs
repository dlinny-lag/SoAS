using System;
using System.Collections.Generic;
using Shared.Utils;

namespace AAF.Services.Differences
{
    public class CollectionComparer<T, TDiff>
        where T: class
        where TDiff: Enum
    {
        private readonly IElementComparer<T, TDiff> elementsComparer;
        public CollectionComparer(IElementComparer<T, TDiff> elementsComparer)
        {
            this.elementsComparer = elementsComparer ?? throw new ArgumentNullException(nameof(elementsComparer));
        }

        /// <summary>
        /// Returns true if all elements in the collection are same
        /// </summary>
        /// <param name="items"></param>
        /// <returns></returns>
        public TDiff AreSame(ICollection<T> items)
        {
            if (items.Count == 0)
                throw new ArgumentOutOfRangeException(nameof(items), "No items to compare");
            if (items.Count == 1)
                return default;
            T reference = null;
            TDiff retVal = default;
            foreach (T item in items)
            {
                if (reference == null)
                {
                    reference = item;
                    continue;
                }
                retVal = retVal.Or(elementsComparer.Same(reference, item));
            }

            return retVal;
        }

        /// <summary>
        /// Returns true if collections are same
        /// </summary>
        /// <returns></returns>
        public bool AreSame(ICollection<T> a, ICollection<T> b)
        {
            if (a.Count != b.Count)
                return false;
            int count = a.Count;
            using (var aItems = a.GetEnumerator())
            {
                using (var bItems = b.GetEnumerator())
                {
                    for (int i = 0; i < count; i++)
                    {
                        aItems.MoveNext();
                        bItems.MoveNext();
                        if (!elementsComparer.Same(aItems.Current, bItems.Current).IsNone())
                            return false;
                    }
                }
            }

            return true;
        }
    }
}