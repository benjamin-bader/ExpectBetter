using System;
using System.Collections.Generic;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on objects implementing
    /// <see cref="IDictionary&lt;K, V&gt;"/>.
    /// </summary>
    /// <typeparam name="K">The type of the dictionary's keys.</typeparam>
    /// <typeparam name="V">The type of the dictionary's values.</typeparam>
    public class DictionaryMatcher<K, V> : BaseCollectionMatcher<IDictionary<K, V>, KeyValuePair<K, V>, DictionaryMatcher<K, V>>
    {
        /// <summary>
        /// Expect the dictionary to contain a given key.
        /// </summary>
        public virtual bool ToContainKey(K expected)
        {
            return actual.ContainsKey(expected);
        }

        /// <summary>
        /// Expect the dictionary to contain a given key-value pair.
        /// </summary>
        public virtual bool ToContainKeyValuePair(KeyValuePair<K, V> expected)
        {
            return actual.Contains(expected);
        }

        /// <summary>
        /// Expect the dictionary to contain a given key and value.
        /// </summary>
        public virtual bool ToContainKeyAndValue(K key, V value)
        {
            V maybeValue;

            if (!actual.TryGetValue(key, out maybeValue))
            {
                return false;
            }

            return value.Equals(maybeValue);
        }
    }
}
