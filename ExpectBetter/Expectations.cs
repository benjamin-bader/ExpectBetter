using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpectBetter
{
    public class Expectations
    {
        /// <summary>
        /// Wraps a matcher in a proxy that checks assertion results, formats
        /// error messages, and performs other housekeeping.
        /// </summary>
        /// <typeparam name="T">
        /// The type of object tested by the wrapped matcher.
        /// </typeparam>
        /// <typeparam name="M">
        /// The type of matcher to be wrapped.
        /// </typeparam>
        /// <param name="actual">
        /// The actual value being tested.
        /// </param>
        /// <returns>
        /// Returns a wrapped matcher that tests the <paramref name="actual"/>
        /// value given.
        /// </returns>
        public static M Wrap<T, M>(T actual)
            where M : BaseMatcher<T, M>
        {
            return ExpectBetter.Codegen.ClassWrapper.Wrap<T, M>(actual);
        }
    }
}
