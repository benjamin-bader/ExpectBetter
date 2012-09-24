using System;
using System.Collections.Generic;

using ExpectBetter.Matchers;

namespace ExpectBetter
{
    /// <summary>
    /// Exposes matcher methods on <see cref="ICollection&lt;TItem&gt;"/>
    /// objects.  Not intended to be exposed directly; use
    /// <see cref="CollectionMatcher&lt;T&gt;"/> instead.
    /// </summary>
    /// <typeparam name="TCollection">
    /// A type implementing <see cref="ICollection&lt;TItem&gt;"/>
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The type of item contained in <typeparamref name="TCollection"/>.
    /// </typeparam>
    /// <typeparam name="M">
    /// The type of the most-derived matcher.
    /// </typeparam>
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
