using System.Collections;
using System.Collections.Generic;

namespace Shared.Utils
{

    /// <summary>
    /// Dictionary that preserves insertion order.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class SequentialDictionary<TKey, TValue> : IDictionary<TKey, TValue>
    {
        private readonly Dictionary<TKey, TValue> storage = new Dictionary<TKey, TValue>();
        private readonly List<TKey> keys = new List<TKey>();

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            foreach (TKey key in Keys)
            {
                yield return new KeyValuePair<TKey, TValue>(key, storage[key]);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        public void Clear()
        {
            keys.Clear();
            storage.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((IDictionary<TKey, TValue>)storage).Contains(item);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            if (!((IDictionary<TKey, TValue>)storage).Remove(item)) 
                return false;

            keys.Remove(item.Key);
            return true;
        }

        public int Count => keys.Count;
        public bool IsReadOnly => false;
        public bool ContainsKey(TKey key)
        {
            return storage.ContainsKey(key);
        }

        public void Add(TKey key, TValue value)
        {
            storage.Add(key, value);
            keys.Add(key); // will not be executed if key is duplicated
        }

        public bool Remove(TKey key)
        {
            if (!storage.TryGetValue(key, out _)) 
                return false;

            keys.Remove(key);
            storage.Remove(key);
            return true;

        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return storage.TryGetValue(key, out value);
        }

        public TValue this[TKey key]
        {
            get => storage[key];
            set
            {
                if (storage.TryGetValue(key, out _))
                    storage[key] = value;
                else
                    Add(key, value);
            }
        }

        public ICollection<TKey> Keys => keys.ToArray();

        public ICollection<TValue> Values
        {
            get
            {
                List<TValue> retVal = new List<TValue>(keys.Count);
                for (int i = 0; i < keys.Count; i++)
                {
                    retVal.Add(storage[keys[i]]);
                }

                return retVal;
            }
        }
    }
}