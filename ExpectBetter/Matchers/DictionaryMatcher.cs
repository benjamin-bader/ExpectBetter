using System.Collections.Generic;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on objects implementing
    /// <see cref="IDictionary&lt;TKey, TValue&gt;"/>.
    /// </summary>
    /// <typeparam name="TKey">The type of the dictionary's keys.</typeparam>
    /// <typeparam name="TValue">The type of the dictionary's values.</typeparam>
    public class DictionaryMatcher<TKey, TValue> : BaseCollectionMatcher<IDictionary<TKey, TValue>, KeyValuePair<TKey, TValue>, DictionaryMatcher<TKey, TValue>>
    {
        /// <summary>
        /// Expect the dictionary to contain a given key.
        /// </summary>
        public virtual bool ToContainKey(TKey expected)
        {
            return actual.ContainsKey(expected);
        }

        /// <summary>
        /// Expect the dictionary to contain a given key-value pair.
        /// </summary>
        public virtual bool ToContainKeyValuePair(KeyValuePair<TKey, TValue> expected)
        {
            return actual.Contains(expected);
        }

        /// <summary>
        /// Expect the dictionary to contain a given key and value.
        /// </summary>
        public virtual bool ToContainKeyAndValue(TKey key, TValue value)
        {
            TValue maybeValue;

            if (!actual.TryGetValue(key, out maybeValue))
            {
                return false;
            }

            return value.Equals(maybeValue);
        }
    }
}
