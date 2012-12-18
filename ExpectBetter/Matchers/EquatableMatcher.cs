using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Provides assertions on objects implementing
    /// <see cref="IEquatable&lt;T&gt;"/>.
    /// </summary>
    public class EquatableMatcher<T> : BaseObjectMatcher<IEquatable<T>, EquatableMatcher<T>>
    {
        /// <summary>
        /// Expect the actual value to be equal to the <paramref name="expected"/>
        /// value.
        /// </summary>
        public virtual bool ToEqual(T expected)
        {
            return actual.Equals(expected);
        }
    }
}
