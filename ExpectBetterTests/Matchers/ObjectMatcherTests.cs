using System;
using System.Collections.Generic;
using System.Net;
using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class ObjectMatcherTests
    {
        [Test]
        public void ToBeNull_WhenActualIsNull_ReturnsTrue()
        {
            Expect.The(null as object).ToBeNull();
        }

        [Test, Throws]
        public void ToBeNull_WhenActualIsNotNull_Throws()
        {
            Expect.The(new object()).ToBeNull();
        }

        [Test, Throws]
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

        [Test, Throws]
        public void ToBeTheSameAs_WhenActualDiffersFromExpected_Throws()
        {
            Expect.The(new object()).ToBeTheSameAs(new TypeLoadException());
        }

        [Test]
        public void ToBeAnInstanceOf_WhenActualHasExpectedType_ReturnsTrue()
        {
            var actual = new Dictionary<string, string>();
            Expect.The(actual).ToBeAnInstanceOf<Dictionary<string, string>>();
        }

        [Test]
        public void ToBeAnInstanceOf_WhenActualImplementsExpectedInterface_ReturnsTrue()
        {
            var actual = new System.Collections.ObjectModel.Collection<int>();
            Expect.The(actual).ToBeAnInstanceOf<IEnumerable<int>>();
        }

        [Test]
        public void ToBeAnInstanceOf_WhenActualInheritsFromExpectedType_ReturnsTrue()
        {
            var request = (HttpWebRequest)WebRequest.CreateDefault(new Uri("http://www.google.com"));
            Expect.The(request).ToBeAnInstanceOf<WebRequest>();
        }
    }
}
