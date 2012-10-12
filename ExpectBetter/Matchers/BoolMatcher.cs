using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods for <see cref="Boolean"/> values.
    /// </summary>
    public class BoolMatcher : BaseMatcher<bool, BoolMatcher>
    {
        /// <summary>
        /// Expect the boolean value to be <see langword="true"/>.
        /// </summary>
        public virtual bool ToBeTrue()
        {
            return actual;
        }

        /// <summary>
        /// Expect the boolean value to be <see langword="false"/>.
        /// </summary>
        public virtual bool ToBeFalse()
        {
            return !actual;
        }

        /// <summary>
        /// Expect the boolean value to match the given value.
        /// </summary>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        public virtual bool ToEqual(bool expected)
        {
            return actual == expected;
        }
    }
}
