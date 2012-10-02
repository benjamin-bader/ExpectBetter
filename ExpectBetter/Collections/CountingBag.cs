using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpectBetter.Collections
{
    /// <summary>
    /// Represents an unordered collection of elements in which duplicates
    /// are allowed.  Suitable only for counting and equality-based membership
    /// checking, as distinct element references are not maintained.
    /// </summary>
    /// <typeparam name="T">
    /// The type of elemented stored.
    /// </typeparam>
    internal class CountingBag<T> : ICollection<T>
    {
        private readonly Dictionary<T, int> dictionary;

        private int totalCount = 0;

        public CountingBag()
            : this(Enumerable.Empty<T>(), EqualityComparer<T>.Default)
        {
        }

        public CountingBag(IEqualityComparer<T> comparer)
            : this(Enumerable.Empty<T>(), comparer)
        {
        }

        public CountingBag(IEnumerable<T> items)
            : this(items, EqualityComparer<T>.Default)
        {
        }

        public CountingBag(IEnumerable<T> items, IEqualityComparer<T> comparer)
        {
            dictionary = new Dictionary<T, int>(comparer);

            foreach (var item in items)
            {
                this.Add(item);
            }
        }

        public bool Contains(T item)
        {
            return dictionary.ContainsKey(item);
        }

        public int Count
        {
            get { return totalCount; }
        }

        public int GetCountFor(T item)
        {
            int count;

            if (!dictionary.TryGetValue(item, out count))
            {
                return 0;
            }

            return count;
        }

        public void Add(T item)
        {
            int count;

            if (!dictionary.TryGetValue(item, out count))
            {
                count = 0;
            }

            ++totalCount;
            dictionary[item] = ++count;
        }

        public bool Remove(T item)
        {
            int count;

            if (!dictionary.TryGetValue(item, out count))
            {
                return false;
            }

            if (count == 1)
            {
                dictionary.Remove(item);
            }
            else
            {
                dictionary[item] = --count;
            }

            --totalCount;
            return true;
        }


        public void Clear()
        {
            totalCount = 0;
            dictionary.Clear();
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            throw new NotSupportedException();
        }

        public bool IsReadOnly
        {
            get { return false; }
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var key in dictionary.Keys)
            {
                var count = dictionary[key];

                while (count > 0)
                {
                    yield return key;
                    --count;
                }
            }
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}
