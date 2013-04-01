using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods for <see cref="Char"/> values.
    /// </summary>
    public class CharMatcher : BaseMatcher<char, CharMatcher>
    {
        /// <summary>
        /// Expect the character to be lower case.
        /// </summary>
        public virtual bool ToBeLowerCase()
        {
            return Char.IsLower(actual);
        }

        /// <summary>
        /// Expect the character to be upper case.
        /// </summary>
        public virtual bool ToBeUpperCase()
        {
            return Char.IsUpper(actual);
        }

        /// <summary>
        /// Expect the character to equal the given character.
        /// </summary>
        /// <param name="expected">
        /// The expected value.
        /// </param>
        public virtual bool ToEqual(char expected)
        {
            return actual == expected;
        }

        /// <summary>
        /// Expect the character to be a letter.
        /// </summary>
        /// <returns></returns>
        public virtual bool ToBeALetter()
        {
            return Char.IsLetter(actual);
        }
    }
}
