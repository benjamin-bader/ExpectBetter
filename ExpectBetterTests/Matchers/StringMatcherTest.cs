using System;

using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Matchers;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class StringMatcherTest
    {
        string actual;
        StringMatcher matcher;

        [SetUp]
        public void Setup()
        {
            actual = Factory.RandomString(20, 5);
            matcher = Expect.The(actual);
        }

        [Test]
        public void ToContain_WhenExpectedIsContained_ReturnsTrue()
        {
            var expectedLen = Math.Min(actual.Length / 2 + 1, actual.Length);
            var expected = actual.Substring(expectedLen);
            var result = matcher.ToContain(expected);

            Expect.The(result).ToBeTrue();
        }

        [Test]
        public void Not_ToContain_WhenExpectedIsNotContained_ReturnsTrue()
        {
            var expected = Factory.RandomString(actual.Length + 10, actual.Length + 1);
            var result = matcher.Not.ToContain(expected);

            Expect.The(result).ToBeTrue();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToContain_WhenExpectedIsNotContained_Throws()
        {
            var expected = Factory.RandomString(actual.Length + 10, actual.Length + 1);

            matcher.ToContain(expected);
        }

        [Test]
        public void ToBeLongerThan_WhenExpectedIsShorter_ReturnsTrue()
        {
            var expected = Factory.RandomString(actual.Length - 1);
            var result = matcher.ToBeLongerThan(expected);

            Expect.The(result).ToBeTrue();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToBeLongerThan_WhenExpectedIsLonger_Throws()
        {
            var expected = Factory.RandomString(actual.Length * 2, actual.Length + 1);
            matcher.ToBeLongerThan(expected);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void Not_ToBeLongerThan_WhenExpectedIsShorter_Throws()
        {
            var expected = Factory.RandomString(actual.Length - 1);
            matcher.Not.ToBeLongerThan(expected);
        }

        [Test]
        public void ToBeLongerThan_WhenExpectedIsLonger_ReturnsTrue()
        {
            var expected = Factory.RandomString(actual.Length * 2, actual.Length + 1);
            var result = matcher.Not.ToBeLongerThan(expected);

            Expect.The(result).ToBeTrue();
        }

        [Test]
        public void ToBeNullOrEmpty_WhenNull_ReturnsTrue()
        {
            var actual = null as string;
            Expect.The(actual).ToBeNullOrEmpty();
        }

        [Test]
        public void ToBeNullOrEmpty_WhenEmpty_ReturnsTrue()
        {
            var actual = string.Empty;
            Expect.The(actual).ToBeNullOrEmpty();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToBeNullOrEmpty_WhenWhitespace_Throws()
        {
            var actual = " ";
            Expect.The(actual).ToBeNullOrEmpty();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToBeNullOrEmpty_WhenNonEmpty_Throws()
        {
            var actual = "?";
            Expect.The(actual).ToBeNullOrEmpty();
        }

        [Test]
        public void ToBeNullOrWhitespace_WhenNull_ReturnsTrue()
        {
            var actual = null as string;
            Expect.The(actual).ToBeNullOrWhitespace();
        }

        [Test]
        public void ToBeNullOrWhitespace_WhenEmpty_ReturnsTrue()
        {
            var actual = string.Empty;
            Expect.The(actual).ToBeNullOrWhitespace();
        }

        [Test]
        public void ToBeNullOrWhitespace_WhenWhitespace_ReturnsTrue()
        {
            var actual = " ";
            Expect.The(actual).ToBeNullOrWhitespace();
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToBeNullOrWhitespace_WhenNonEmpty_Throws()
        {
            var actual = "?";
            Expect.The(actual).ToBeNullOrWhitespace();
        }
    }
}
