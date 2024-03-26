using System;
using System.Collections;
using System.Collections.Generic;

namespace Shared.Utils
{
    public static class CollectionExtensions
    {
        public static void AddRange<T>(this HashSet<T> set, IEnumerable<T> list)
        {
            foreach (T e in list)
            {
                set.Add(e);
            }
        }

        public static void RemoveRange<T>(this HashSet<T> set, IEnumerable<T> list)
        {
            foreach (T e in list)
            {
                set.Remove(e);
            }
        }

        public static T Min<T>(this IEnumerable<T> list)
            where T:struct, IComparable<T>
        {
            T retVal = default;
            foreach (var value in list)
            {
                if (retVal.CompareTo(value) < 0)
                    retVal = value;
            }

            return retVal;
        }

        public static void RemoveRange<T>(this List<T> list, IEnumerable<T> toRemove)
        {
            foreach (T e in toRemove)
            {
                list.Remove(e);
            }
        }

        public static bool IsIn<T, V>(this T key, IDictionary<T, V> dict)
        {
            return dict.ContainsKey(key);
        }
        public static bool IsIn<T>(this T key, ICollection<T> set)
        {
            return set.Contains(key);
        }

        public static bool ContainsAny<T>(this ICollection<T> list, params T[] values)
        {
            for (int i = 0; i < values.Length; i++)
            {
                if (list.Contains(values[i]))
                    return true;
            }

            return false;
        }

        public static List<T> With<T>(this List<T> toModify, params T[] toAdd)
            where T: class
        {
            foreach (T val in toAdd)
            {
                if (val == null)
                    continue;
                toModify.Add(val);
            }
            return toModify;
        }
        public static List<T> With<T>(this List<T> toModify, int startIndex, params T[] toAdd)
            where T: class
        {

            foreach (T val in toAdd)
            {
                if (val == null)
                    continue;
                toModify.Insert(startIndex++, val);
            }
            return toModify;
        }

        public static T TryGet<T>(this IList<T> list, int index)
            where T : class
        {
            if (list == null)
                return null;
            if (index < 0 || index > list.Count - 1)
                return null;
            return list[index];
        }
    }
}
