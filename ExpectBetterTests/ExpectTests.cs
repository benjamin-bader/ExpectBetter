using System;
using System.Reflection;
using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Matchers;

namespace ExpectBetterTests
{
    [TestFixture]
    public class ExpectTests
    {
        [Test]
        public void Expect_The_CanDistinguishValueTypes()
        {
            byte b = 0;
            sbyte sb = 0;
            short s = 0;
            ushort us = 0;
            int i = 0;
            uint ui = 0u;
            long l = 0;
            ulong ul = 0ul;
            float f = 0.0f;
            double d = 0.0d;
            decimal m = 0.0M;
            IntPtr ip = IntPtr.Zero;
            UIntPtr uip = UIntPtr.Zero;

            Expect.The(Expect.The(b)).ToBeAnInstanceOf<ByteMatcher>();
            Expect.The(Expect.The(sb)).ToBeAnInstanceOf<SByteMatcher>();
            Expect.The(Expect.The(s)).ToBeAnInstanceOf<Int16Matcher>();
            Expect.The(Expect.The(us)).ToBeAnInstanceOf<UInt16Matcher>();
            Expect.The(Expect.The(i)).ToBeAnInstanceOf<Int32Matcher>();
            Expect.The(Expect.The(ui)).ToBeAnInstanceOf<UInt32Matcher>();
            Expect.The(Expect.The(l)).ToBeAnInstanceOf<Int64Matcher>();
            Expect.The(Expect.The(ul)).ToBeAnInstanceOf<UInt64Matcher>();
            Expect.The(Expect.The(f)).ToBeAnInstanceOf<SingleMatcher>();
            Expect.The(Expect.The(d)).ToBeAnInstanceOf<DoubleMatcher>();
            Expect.The(Expect.The(m)).ToBeAnInstanceOf<DecimalMatcher>();
            Expect.The(Expect.The(ip)).ToBeAnInstanceOf<IntPtrMatcher>();
            Expect.The(Expect.The(uip)).ToBeAnInstanceOf<UIntPtrMatcher>();
        }

        [Test]
        public void The_AllowsNegation()
        {
            var stringMatcher = Expect.The("foo");
            var negation = stringMatcher.Not;

            Expect.The(negation).Not.ToBeNull();
        }

        [Test]
        public void The_AllowsDoubleNegatives()
        {
            var collectionMatcher = Expect.The(new[] { 1, 2, 3 });
            var negation = collectionMatcher.Not;
            var doubleNegation = negation.Not;

            Expect.The(doubleNegation).ToBeTheSameAs(collectionMatcher);
        }

        [Test]
        public void The_ForAnyInput_ReturnsMatcherSubclass()
        {
            var declaredMatcherType = typeof (Expect).GetMethod("The", new[] {typeof (string)}).ReturnType;
            var actualMatcherType = Expect.The("foo").GetType();

            Expect.The(actualMatcherType).ToInheritFrom(declaredMatcherType);
        }

        [Test]
        public void The_WhenMatcherHasConstructor_CallsTheConstructor()
        {
            var matcher = Expectations.Wrap<byte, Matcher>(0);
            Expect.The(matcher.CustomCtorCalled).ToBeTrue();
            Expect.The(matcher.DefaultCtorCalled).ToBeFalse();
        }

        public class Matcher : BaseMatcher<byte, Matcher>
        {
            private readonly bool customCtorCalled;
            private readonly bool defaultCtorCalled;

            public bool CustomCtorCalled
            {
                get { return customCtorCalled; }
            }

            public bool DefaultCtorCalled
            {
                get { return defaultCtorCalled; }
            }

            public Matcher()
            {
                customCtorCalled = false;
                defaultCtorCalled = true;
            }

            public Matcher(byte actual)
            {
                customCtorCalled = true;
                defaultCtorCalled = false;
            }
        }
    }
}
