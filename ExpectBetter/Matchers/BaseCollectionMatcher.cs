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
    /// <typeparam name="TMatcher">
    /// The type of the most-derived matcher.
    /// </typeparam>
    public class BaseCollectionMatcher<TCollection, TItem, TMatcher> : BaseEnumerableMatcher<TCollection, TItem, TMatcher>
        where TCollection : ICollection<TItem>
        where TMatcher : BaseCollectionMatcher<TCollection, TItem, TMatcher>
    {
        /// <summary>
        /// Expect the collection to have a given number of elements.
        /// </summary>
        public virtual bool ToNumber(int expectedCount)
        {
            return actual.Count == expectedCount;
        }

        /// <summary>
        /// Expect the collection to have at minimum a given number of elements.
        /// </summary>
        public virtual bool ToNumberAtLeast(int expected)
        {
            return actual.Count >= expected;
        }

        /// <summary>
        /// Expect the collection to have no elements.
        /// </summary>
        /// <returns></returns>
        public override bool ToBeEmpty()
        {
            return actual.Count == 0;
        }

        /// <summary>
        /// Expect the collection to have a given item.
        /// </summary>
        public override bool ToContain(TItem item)
        {
            return actual.Contains(item);
        }
    }
}
