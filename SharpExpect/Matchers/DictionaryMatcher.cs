using System;
using System.Collections.Generic;

namespace SharpExpect.Matchers
{
	public class DictionaryMatcher<K, V> : BaseCollectionMatcher<IDictionary<K, V>, KeyValuePair<K, V>, DictionaryMatcher<K, V>>
	{
		public virtual bool ToContainKey(K expected)
		{
			return actual.ContainsKey(expected);
		}

		public virtual bool ToContainKeyValuePair(KeyValuePair<K, V> expected)
		{
			return actual.Contains(expected);
		}

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

