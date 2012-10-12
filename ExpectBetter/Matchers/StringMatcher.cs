using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on strings.
    /// </summary>
    public class StringMatcher : BaseObjectMatcher<string, StringMatcher>
    {
        /// <summary>
        /// Expect the string to be longer than a given string.
        /// </summary>
        public virtual bool ToBeLongerThan(string expected)
        {
            return actual.Length > expected.Length;
        }

        /// <summary>
        /// Expect the string to equal a given value using the provided
        /// comparison.
        /// </summary>
        public virtual bool ToEqual(string expected, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return string.Equals(actual, expected, comparison);
        }

        /// <summary>
        /// Expect the string to contain a given value using the provided
        /// comparison.
        /// </summary>
        public virtual bool ToContain(string expected, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
        {
            return actual.IndexOf(expected, comparison) >= 0;
        }

        /// <summary>
        /// Expect the string to be null or empty.
        /// </summary>
        [AllowNullActual]
        public virtual bool ToBeNullOrEmpty()
        {
            return string.IsNullOrEmpty(actual);
        }

        /// <summary>
        /// Expect the string to be null, empty or whitespace.
        /// </summary>
        [AllowNullActual]
        public virtual bool ToBeNullOrWhitespace()
        {
            return string.IsNullOrWhiteSpace(actual);
        }
    }
}
