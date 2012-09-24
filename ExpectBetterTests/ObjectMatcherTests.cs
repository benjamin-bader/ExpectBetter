using System;
using System.Collections.Generic;
using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Matchers;

namespace ExpectBetterTests
{
    [TestFixture]
    public class ObjectMatcherTests
    {
        [Test]
        public void ToBeNull_WhenActualIsNull_ReturnsTrue()
        {
            Expect.The(null as object).ToBeNull();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToBeNull_WhenActualIsNotNull_Throws()
        {
            Expect.The(new object()).ToBeNull();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void Not_ToBeNull_WhenActualIsNull_Throws()
        {
            Expect.The(null as object).Not.ToBeNull();
        }

        [Test]
        public void Not_ToBeNull_WhenActualIsNotNull_ReturnsTrue()
        {
            Expect.The(new object()).Not.ToBeNull();
        }

        [Test]
        public void ToBeTheSameAs_WhenActualReferenceEqualsExpected_ReturnsTrue()
        {
            var actual = new object();
            var expected = actual;

            Expect.The(actual).ToBeTheSameAs(expected);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToBeTheSameAs_WhenActualDiffersFromExpected_Throws()
        {
            Expect.The(new object()).ToBeTheSameAs(new TypeLoadException());
        }

        [Test]
        public void ToBeAnInstanceOf_WhenActualHasExpectedType_ReturnsTrue()
        {
            var actual = new Dictionary<string, string>();

            Expect.The(actual).ToBeAnInstanceOf<IDictionary<string, string>>();
        }
    }
}
