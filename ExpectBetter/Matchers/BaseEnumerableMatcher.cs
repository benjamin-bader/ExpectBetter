using System.Collections.Generic;
using System.Linq;

using ExpectBetter.Collections;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes matcher methods on <see cref="IEnumerable&lt;TItem&gt;"/>
    /// objects.  Not intended to be exposed directly; use
    /// <see cref="EnumerableMatcher&lt;T&gt;"/> instead.
    /// </summary>
    /// <typeparam name="TEnumerable">
    /// The test type, implementing <see cref="IEnumerable&lt;TItem&gt;"/>.
    /// </typeparam>
    /// <typeparam name="TItem">
    /// The type of element contained in <typeparamref name="TEnumerable"/>.
    /// </typeparam>
    /// <typeparam name="TMatcher">
    /// The type of the most-derived matcher.
    /// </typeparam>
    public class BaseEnumerableMatcher<TEnumerable, TItem, TMatcher> : BaseObjectMatcher<TEnumerable, TMatcher>
        where TEnumerable : IEnumerable<TItem>
        where TMatcher : BaseEnumerableMatcher<TEnumerable, TItem, TMatcher>
    {
        /// <summary>
        /// Expect the enumerable to contain no elements.
        /// </summary>
        public virtual bool ToBeEmpty()
        {
            return !actual.Any();
        }

        /// <summary>
        /// Expect the enumerable to contain the given item.
        /// </summary>
        /// <remarks>
        /// Uses the default equality comparer for <typeparamref name="TItem"/>.
        /// </remarks>
        /// <param name="expected">
        /// The expected item.
        /// </param>
        public virtual bool ToContain(TItem expected)
        {
            return ToContain(expected, EqualityComparer<TItem>.Default);
        }

        /// <summary>
        /// Expect the enumerable to contain the given item, using a provided
        /// equality comparer.
        /// </summary>
        /// <param name="expected">
        /// The expected item.
        /// </param>
        /// <param name="comparer">
        /// The equality comparer to use in testing for item membership.
        /// </param>
        public virtual bool ToContain(TItem expected, IEqualityComparer<TItem> comparer)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TItem>.Default;
            }

            return actual.Any(element => comparer.Equals(element, expected));
        }

        /// <summary>
        /// Expect the enumerable to contain a number of items in the given
        /// ordering.  Other items in between are acceptable.
        /// </summary>
        /// <remarks>
        /// Uses the default equality comparer for <typeparamref name="TItem"/>.
        /// </remarks>
        /// <param name="expected">
        /// The items expected to be present.
        /// </param>
        public virtual bool ToContainInOrder(IEnumerable<TItem> expected)
        {
            return ToContainInOrder(expected, EqualityComparer<TItem>.Default);
        }

        /// <summary>
        /// Expect the enumerable to contain a number of items in the given
        /// ordering.  Other items in between are acceptable.
        /// </summary>
        /// <remarks>
        /// Uses the default equality comparer for <typeparamref name="TItem"/>.
        /// </remarks>
        /// <param name="expected">
        /// The items expected to be present.
        /// </param>
        public virtual bool ToContainInOrder(params TItem[] expected)
        {
            return ToContainInOrder(expected, EqualityComparer<TItem>.Default);
        }

        /// <summary>
        /// Expect the enumerable to contain a number of items in the given
        /// ordering, using the given equality comparer.  Other items in
        /// between are acceptable.
        /// </summary>
        /// <param name="expected">
        /// The items expected to be present.
        /// </param>
        /// <param name="comparer">
        /// The equality comparer to use in testing for item membership.
        /// </param>
        public virtual bool ToContainInOrder(IEnumerable<TItem> expected, IEqualityComparer<TItem> comparer)
        {
            return EnumerateTwo(actual, expected, (actualEnumerator, expectedEnumerator) =>
            {
                var hasMoreExpected = expectedEnumerator.MoveNext();

                if (comparer == null)
                {
                    comparer = EqualityComparer<TItem>.Default;
                }

                while (hasMoreExpected && actualEnumerator.MoveNext())
                {
                    if (comparer.Equals(actualEnumerator.Current, expectedEnumerator.Current))
                    {
                        hasMoreExpected = expectedEnumerator.MoveNext();
                    }
                }

                return !hasMoreExpected;
            });
        }

        /// <summary>
        /// Expect the enumerable to contain only the given items.
        /// </summary>
        /// <param name="expected">
        /// The expected items.
        /// </param>
        public virtual bool ToContainExactly(IEnumerable<TItem> expected)
        {
            return ToContainExactly(expected, EqualityComparer<TItem>.Default);
        }

        /// <summary>
        /// Expect the enumerable to contain only the given items.
        /// </summary>
        /// <param name="expected">
        /// The expected items.
        /// </param>
        public virtual bool ToContainExactly(params TItem[] expected)
        {
            return ToContainExactly(expected, EqualityComparer<TItem>.Default);
        }

        /// <summary>
        /// Expect the enumerable to contain only the given items, using the
        /// given equality comparer.
        /// </summary>
        /// <param name="expected">
        /// The expected items.
        /// </param>
        /// <param name="comparer">
        /// The comparer used to determine item membership.
        /// </param>
        public virtual bool ToContainExactly(IEnumerable<TItem> expected, IEqualityComparer<TItem> comparer)
        {
            var bag = new CountingBag<TItem>(actual, comparer);

            if (expected.Any(item => !bag.Remove(item)))
            {
                return false;
            }

            return bag.Count == 0;
        }

        /// <summary>
        /// Expect the enumerable to be a sequence of the same length and having equal
        /// elements as the given sequence.
        /// </summary>
        /// <param name="expected">
        /// The expected items.
        /// </param>
        /// <param name="comparer">
        /// The comparer used to determine item equality.  If <see langword="null"/>,
        /// <see cref="EqualityComparer{TItem}.Default"/> is used.
        /// </param>
        public virtual bool ToEnumerateAs(IEnumerable<TItem> expected, IEqualityComparer<TItem> comparer = null)
        {
            if (comparer == null)
            {
                comparer = EqualityComparer<TItem>.Default;
            }

            using (var one = actual.GetEnumerator())
            using (var two = expected.GetEnumerator())
            {
                bool hasOne, hasTwo;
                do
                {
                    hasOne = one.MoveNext();
                    hasTwo = two.MoveNext();

                    if (!hasOne || !hasTwo)
                    {
                        break;
                    }

                    if (!comparer.Equals(one.Current, two.Current))
                    {
                        return false;
                    }
                }
                while (hasOne && hasTwo);

                return hasOne == hasTwo;
            }
        }

        private delegate bool DualEnumeratorPredicate(IEnumerator<TItem> actualEnumerator, IEnumerator<TItem> expectedEnumerator);

        /// <summary>
        /// Applies a predicate to two enumerables and returns the result.
        /// </summary>
        /// <param name="actual"></param>
        /// <param name="expected"></param>
        /// <param name="method"></param>
        /// <returns></returns>
        private static bool EnumerateTwo(IEnumerable<TItem> actual, IEnumerable<TItem> expected, DualEnumeratorPredicate method)
        {
            IEnumerator<TItem> actualEnumerator = null,
                               expectedEnumerator = null;

            try
            {
                actualEnumerator = actual.GetEnumerator();
                expectedEnumerator = expected.GetEnumerator();

                return method(actualEnumerator, expectedEnumerator);
            }
            finally
            {
                if (actualEnumerator != null)
                {
                    actualEnumerator.Dispose();
                }

                if (expectedEnumerator != null)
                {
                    expectedEnumerator.Dispose();
                }
            }
        }
    }
}
