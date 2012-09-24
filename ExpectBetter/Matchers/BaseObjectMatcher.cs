using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes matcher methods on <see cref="Object"/>.  Not intended to be
    /// exposed directly; use <see cref="ObjectMatcher"/> instead.
    /// </summary>
    /// <typeparam name="T">
    /// The type being tested.
    /// </typeparam>
    /// <typeparam name="M">
    /// The type of the most-derived matcher.
    /// </typeparam>
    public class BaseObjectMatcher<T, M> : BaseMatcher<T, M>
        where M : BaseObjectMatcher<T, M>
    {
        [AllowNullActual]
        public virtual bool ToBeNull()
        {
            return ReferenceEquals(null, actual);
        }

        [AllowNullActual]
        public virtual bool ToBeTheSameAs(T expected)
        {
            return ReferenceEquals(actual, expected);
        }

        public virtual bool ToEqual(T expected)
        {
            return actual.Equals(expected);
        }

        public virtual bool ToBeAnInstanceOf<TExpected>()
        {
            expectedDescription = typeof(TExpected).FullName;
            return typeof(TExpected).IsAssignableFrom(actual.GetType());
        }
    }
}
