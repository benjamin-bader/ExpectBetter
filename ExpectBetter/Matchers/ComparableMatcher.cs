using System;

using ExpectBetter.Matchers;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on objects implementing
    /// <see cref="IComparable&lt;T&gt;"/>.
    /// </summary>
    /// <typeparam name="T">
    /// The type of object to which the object being tested can be compared.
    /// </typeparam>
    public class ComparableMatcher<T> : BaseObjectMatcher<IComparable<T>, ComparableMatcher<T>>
    {
        public virtual bool ToEqual(T expected)
        {
            return actual.CompareTo(expected) == 0;
        }

        public virtual bool ToBeLessThan(T expected)
        {
            return actual.CompareTo(expected) < 0;
        }

        public virtual bool ToBeGreaterThan(T expected)
        {
            return actual.CompareTo(expected) > 0;
        }
    }
}
