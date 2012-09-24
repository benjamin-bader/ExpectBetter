using System;
using System.Collections.Generic;

using ExpectBetter.Matchers;

namespace ExpectBetter
{
    public class BaseCollectionMatcher<TCollection, TItem, M> : BaseEnumerableMatcher<TCollection, TItem, M>
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

        public override bool ToBeEmpty()
        {
            return actual.Count == 0;
        }

        public override bool ToContain(TItem item)
        {
            return actual.Contains(item);
        }
    }
}
