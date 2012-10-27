using System;
using System.Collections.Generic;
using System.Net;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class TypeMatcherTests
    {
        [Test, Throws]
        public void ToInheritFrom_WhenActualIsSameTypeAsExpected_Throws()
        {
            var actual = typeof (TestFixtureAttribute);
            Expect.The(actual).ToInheritFrom<TestFixtureAttribute>();
        }

        [Test, Throws]
        public void ToInheritFrom_WhenActualDoesNotInherit_ThrowsExpectationException()
        {
            Expect.The(typeof (TestFixtureAttribute)).ToInheritFrom<List<string>>();
        }

        [Test]
        public void ToInheritFrom_WhenActualImplementsExpectedInterface_ReturnsTrue()
        {
            Expect.The(typeof (List<string>)).ToInheritFrom<IList<string>>();
        }

        [Test]
        public void ToInheritFrom_WhenActualExtendsExpectedClass_ReturnsTrue()
        {
            Expect.The(typeof (HttpWebRequest)).ToInheritFrom<WebRequest>();
        }

        [Test, Throws]
        public void ToInheritFrom_NonGeneric_WhenActualIsSameType_Throws()
        {
            var actual = typeof (TestFixtureAttribute);
            Expect.The(actual).ToInheritFrom(actual);
        }

        [Test, Throws]
        public void ToInheritFrom_NonGeneric_WhenActualDoesNotInherit_ThrowsExpectationException()
        {
            Expect.The(typeof (TestFixtureAttribute)).ToInheritFrom(typeof (List<string>));
        }

        [Test]
        public void ToInheritFrom_NonGeneric_WhenActualImplementsExpectedInterface_ReturnsTrue()
        {
            Expect.The(typeof(List<string>)).ToInheritFrom(typeof (IList<string>));
        }

        [Test]
        public void ToInheritFrom_NonGeneric_WhenActualExtendsExpectedClass_ReturnsTrue()
        {
            Expect.The(typeof(HttpWebRequest)).ToInheritFrom(typeof (WebRequest));
        }

        [Test]
        public void ToBeGeneric_WhenActualHasGenericParameters_ReturnsTrue()
        {
            Expect.The(typeof (IList<string>)).ToBeGeneric();
        }

        [Test]
        public void ToBeGeneric_WhenActualIsOpenType_ReturnsTrue()
        {
            Expect.The(typeof (IList<>)).ToBeGeneric();
        }

        [Test, Throws]
        public void ToBeGeneric_WhenActualIsNotGeneric_Throws()
        {
            Expect.The(typeof (string)).ToBeGeneric();
        }

        [Test]
        public void ToBeAGenericTypeDefinition_WhenActualIsGenericTypeDefinition_ReturnsTrue()
        {
            var type = typeof (IList<>);
            Expect.The(type).ToBeAGenericTypeDefinition();
        }

        [Test, Throws]
        public void ToBeAGenericTypeDefinition_WhenActualHasGenericParameters_Throws()
        {
            var type = typeof(IList<string>);
            Expect.The(type).ToBeAGenericTypeDefinition();
        }

        [Test, Throws]
        public void ToBeAGenericTypeDefinition_WhenActualIsNotGeneric_Throws()
        {
            var type = typeof(BitConverter);
            Expect.The(type).ToBeAGenericTypeDefinition();
        }
    }
}