using System;
using System.Collections.Generic;
using SharpExpect.Matchers;

namespace SharpExpect
{
	public class BaseCollectionMatcher<TCollection, TItem, M> : BaseObjectMatcher<TCollection, M>
		where TCollection : ICollection<TItem>
		where M : BaseCollectionMatcher<TCollection, TItem, M>
	{
		public virtual bool ToNumber(int expectedCount)
		{
			return actual.Count == expectedCount;
		}

		public virtual bool ToNumberAtLeast(int expected)
		{
			return actual.Count >= expected;
		}

		public virtual bool ToBeEmpty()
		{
			return actual.Count == 0;
		}
	}
}

