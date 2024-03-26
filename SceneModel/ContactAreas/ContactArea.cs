using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace SceneModel.ContactAreas
{
    [DebuggerDisplay("{Id}")]
    public abstract class ContactArea
    {
        private static readonly Dictionary<string, Type> map;

        static ContactArea()
        {
            var rootType = typeof(ContactArea);
            var declared = rootType.Assembly.GetTypes().Where(t => !t.IsAbstract && rootType.IsAssignableFrom(t));
            map = declared.ToDictionary(t => t.Name);
        }

        public string AsString()
        {
            return Id;
        }

        public static ContactArea FromString(string toConvert)
        {
            string[] parts = toConvert.Split(Delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
            if (parts.Length == 0 || parts.Length > 2)
                throw new InvalidOperationException("Wrong format");
            string prefix;
            string typeName;
            if (parts.Length == 1)
            {
                prefix = "Any";
                typeName = parts[0];
            }
            else
            {
                prefix = parts[0];
                typeName = parts[1];
            }
            if (!map.TryGetValue(typeName, out var type))
                throw new InvalidOperationException($"Can't find class {typeName}");
            var fi = type.GetField(prefix, BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);
            if (fi == null)
                throw new InvalidOperationException($"Failed to find contact area for {toConvert}");
            return fi.GetValue(null) as ContactArea;
        }

        public const string Delimiter = " ";
        public virtual string DisplayName => GetType().Name;
        public string Id => $"{Prefix}{Delimiter}{GetType().Name}".Trim();
        public string DisplayId => $"{Prefix}{Delimiter}{DisplayName}".Trim();
        public abstract string Prefix { get; }

        public abstract ContactArea[] GetVariants();

        public abstract bool IsAny { get; }
    }

    public abstract class ContactArea<TLocationness> : ContactArea
        where TLocationness : Locationness
    {
        protected ContactArea(TLocationness locationness)
        {
            Locationness = locationness;
        }
        public TLocationness Locationness { get; }

        public sealed override string Prefix => Locationness.Prefix;

        public sealed override bool IsAny => Locationness.IsAny;

        public sealed override int GetHashCode()
        {
            throw new InvalidOperationException("Must not be used as a key"); // prevent usage in unordered collections
        }

        private static readonly ConcurrentDictionary<Type, List<ContactArea<TLocationness>>> cache = new ConcurrentDictionary<Type, List<ContactArea<TLocationness>>>();

        public override ContactArea[] GetVariants()
        {
            var retVal = cache.GetOrAdd(GetType(), GetSortedVariants);
            // returning value is not supposed to be modified
            // ReSharper disable once CoVariantArrayConversion
            return retVal.ToArray();
        }

        private static List<ContactArea<TLocationness>> GetSortedVariants(Type thisType)
        {
            var props = thisType.GetFields(BindingFlags.Public | BindingFlags.DeclaredOnly | BindingFlags.Static);
            List<ContactArea<TLocationness>> retVal = new List<ContactArea<TLocationness>>(
                    props.Select(p => (ContactArea<TLocationness>)p.GetValue(null)));
            SortVariants(retVal);
            return retVal;
        }

        private static void SortVariants(List<ContactArea<TLocationness>> list)
        {
            if (typeof(SingleSymmetryLocationness).IsAssignableFrom(typeof(TLocationness)))
                SortByLocationness<SingleSymmetryLocationness>(list, SingleSymmetryComparer);
            else if (typeof(DoubleSymmetryLocationness).IsAssignableFrom(typeof(TLocationness)))
                SortByLocationness<DoubleSymmetryLocationness>(list, DoubleSymmetryComparer);
            else
                throw new InvalidOperationException($"Unsupported locationness type {typeof(TLocationness).FullName}");
        }

        private static void SortByLocationness<TL>(List<ContactArea<TLocationness>> list, Func<TL, TL, int> comparer)
            where TL : Locationness
        {
            list.Sort((a, b) =>
            {
                TL al = a.Locationness as TL;
                if (al == null)
                    throw new InvalidCastException($"Can't cast {typeof(TLocationness).Name} to {typeof(TL).Name}");
                TL bl = b.Locationness as TL;
                if (bl == null)
                    throw new InvalidCastException($"Can't cast {typeof(TLocationness).Name} to {typeof(TL).Name}");
                return comparer(al, bl);
            });
        }

        private static int DoubleSymmetryComparer(DoubleSymmetryLocationness a, DoubleSymmetryLocationness b)
        {
            int result1 = a.Code1.CompareTo(b.Code1);
            if (result1 != 0)
                return result1;
            return a.Code2.CompareTo(b.Code2);
        }

        
        private static int SingleSymmetryComparer(SingleSymmetryLocationness a, SingleSymmetryLocationness b)
        {
            return a.Code.CompareTo(b.Code);
        }
    }

    public abstract class AreaReference
    {
        protected AreaReference(ContactArea area)
        {
            Area = area;
        }
        public ContactArea Area { get; }
    }

}