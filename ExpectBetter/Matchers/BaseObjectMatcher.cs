using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes matcher methods on <see cref="Object"/>.  Not intended to be
    /// exposed directly; use <see cref="ObjectMatcher"/> instead.
    /// </summary>
    /// <typeparam name="TActual">
    /// The type being tested.
    /// </typeparam>
    /// <typeparam name="TMatcher">
    /// The type of the most-derived matcher.
    /// </typeparam>
    public class BaseObjectMatcher<TActual, TMatcher> : BaseMatcher<TActual, TMatcher>
        where TMatcher : BaseObjectMatcher<TActual, TMatcher>
    {
        /// <summary>
        /// Expect the value to be <see langword="null"/>.
        /// </summary>
        [AllowNullActual]
        public virtual bool ToBeNull()
        {
            return ReferenceEquals(null, actual);
        }

        /// <summary>
        /// Expect the value to be the same instance as the given
        /// value.
        /// </summary>
        /// <param name="expected">
        /// The expected instance.
        /// </param>
        [AllowNullActual]
        public virtual bool ToBeTheSameAs(TActual expected)
        {
            return ReferenceEquals(actual, expected);
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        public virtual bool ToEqual(TActual expected)
        {
            return actual.Equals(expected);
        }

        /// <summary>
        /// Expect the value to be an instance of
        /// <typeparamref name="TExpected"/>.
        /// </summary>
        /// <remarks>
        /// This is true if an instance of <typeparamref name="TExpected"/> is
        /// assignable from an instance of the runtime type of the value.
        /// </remarks>
        /// <typeparam name="TExpected">
        /// The expected type of the value.
        /// </typeparam>
        public virtual bool ToBeAnInstanceOf<TExpected>()
        {
            expectedDescription = typeof(TExpected).FullName;

            return actual is TExpected;
        }
    }
}
