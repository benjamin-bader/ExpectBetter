using System;
using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class ComparableMatcherTests
    {
        [Test]
        public void ToEqual_WhenComparableIsEqualToExpected_ReturnsTrue()
        {
            var comparable = Comparable<string>.Equal();
            Expect.The(comparable).ToEqual("foo");
        }

        [Test, Throws]
        public void ToEqual_WhenComparableIsLessThanExpected_Throws()
        {
            var comparable = Comparable<string>.Lesser();
            Expect.The(comparable).ToEqual("foo");
        }

        [Test, Throws]
        public void ToEqual_WhenComparableIsGreaterThanExpected_Throws()
        {
            var comparable = Comparable<string>.Greater();
            Expect.The(comparable).ToEqual("foo");
        }

        [Test, Throws]
        public void ToBeLessThan_WhenComparableIsEqualToExpected_Throws()
        {
            var comparable = Comparable<string>.Equal();
            Expect.The(comparable).ToBeLessThan("bar");
        }

        [Test]
        public void ToBeLessThan_WhenComparableIsLessThanExpected_ReturnsTrue()
        {
            var comparable = Comparable<string>.Lesser();
            Expect.The(comparable).ToBeLessThan("bar");
        }

        [Test, Throws]
        public void ToBeLessThan_WhenComparableIsGreaterThanExpected_Throws()
        {
            var comparable = Comparable<string>.Greater();
            Expect.The(comparable).ToBeLessThan("bar");
        }

        [Test, Throws]
        public void ToBeGreaterThan_WhenComparableIsEqualToExpected_Throws()
        {
            var comparable = Comparable<string>.Equal();
            Expect.The(comparable).ToBeGreaterThan("quux");
        }

        [Test, Throws]
        public void ToBeGreaterThan_WhenComparableIsLessThanExpected_ReturnsTrue()
        {
            var comparable = Comparable<string>.Lesser();
            Expect.The(comparable).ToBeGreaterThan("quux");
        }

        [Test]
        public void ToBeGreaterThan_WhenComparableIsGreaterThanExpected_Throws()
        {
            var comparable = Comparable<string>.Greater();
            Expect.The(comparable).ToBeGreaterThan("quux");
        }

        private class Comparable<T> : IComparable<T>
        {
            public static Comparable<T> Lesser()
            {
                return new Comparable<T>(-1);
            }

            public static Comparable<T> Equal()
            {
                return new Comparable<T>(0);
            }

            public static Comparable<T> Greater()
            {
                return new Comparable<T>(1);
            }

            private readonly int compareResult;

            private Comparable(int compareResult)
            {
                this.compareResult = compareResult;
            }

            public int CompareTo(T other)
            {
                return compareResult;
            }

            public override string ToString()
            {
                if (compareResult == 0)
                {
                    return "equivalent object";
                }
                
                if (compareResult < 0)
                {
                    return "lesser object";
                }

                return "greater object";
            }
        }
    }
}
