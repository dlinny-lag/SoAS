using System;
using System.Collections;
using System.Collections.Generic;
using AAFModel;

namespace AAF.Services.AAFImport
{
    public class DuplicationsHolder<T> : ICollection<T>
        where T: Declared
    {
        private static readonly string typeName = typeof(T).Name;

        private readonly List<T> duplicates;
        
        public DuplicationsHolder(string id, T reference)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            duplicates = new List<T>(4);
            AddDuplicates(reference);
        }

        public readonly string Id;

        private T reference;
        public T Reference
        {
            get => reference ?? duplicates[0];
            set => reference = value;
        }

        public int Count => duplicates.Count;
        public T this[int index] => duplicates[index];
        
        public void Validate()
        {
            if (duplicates.Count < 2)
                throw new InvalidOperationException("No duplications provided");
        }

        public void AddDuplicates(params T[] duplications)
        {
            for (int i = 0; i < duplications.Length; i++)
            {
                if (Exist(duplications[i]))
                    continue;
                if (Id != duplications[i].Id)
                    throw new ArgumentOutOfRangeException(nameof(duplications),
                        $"Expected {typeName} with id={Id}, but {duplications[i].Id} provided");
                duplicates.Add(duplications[i]);
            }
        }

        private bool Exist(T anim)
        {
            for (int i = 0; i < duplicates.Count; i++)
            {
                if (ReferenceEquals(duplicates[i], anim))
                    return true;
            }

            return false;
        }

        #region ICollection
        public bool IsReadOnly => true;

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < duplicates.Count; i++)
            {
                if (i == 0)
                    yield return Reference;
                else
                    yield return duplicates[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
        public void Add(T item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(T item)
        {
            return Exist(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public bool Remove(T item)
        {
            throw new NotImplementedException();
        }

        #endregion
        
    }
}