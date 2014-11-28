using System;
using System.Collections.Generic;
using System.Linq;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class EnumerableMatcherTests
    {
        [Test]
        public void ToContainInOrder_WhenTrue_ReturnsTrue()
        {
            var collection = new[] { 1, 2, 3, 4, 5, 6, 7, 8 }.AsEnumerable();
            Expect.The(collection).ToContainInOrder(2, 4, 6);
        }

        [Test, Throws]
        public void ToContainInOrder_WhenNotContainedAtAll_Throws()
        {
            var collection = new[] { 1, 2, 3, 4, 5 }.AsEnumerable();
            Expect.The(collection).ToContainInOrder(7);
        }

        [Test, Throws]
        public void ToContainInOrder_WhenContained_InWrongOrder_Throws()
        {
            var collection = new[] { 2, 3, 4 }.AsEnumerable();
            Expect.The(collection).ToContainInOrder(4, 2);
        }

        [Test]
        public void ToContainExactly_WhenContentsMatch_ReturnsTrue()
        {
            var collection = new[] { "a", "b", "c" }.AsEnumerable();

            Expect.The(collection).ToContainExactly("a", "b", "c");
        }

        [Test]
        public void ToContainExactly_WhenContentsOutOfOrder_ReturnsTrue()
        {
            var collection = new[] { "a", "b", "c" }.AsEnumerable();

            Expect.The(collection).ToContainExactly("a", "c", "b");
        }

        [Test]
        public void ToContainExactly_AccountForDuplicates()
        {
            var collection = new[] { "a", "b", "b", "c" }.AsEnumerable();
            Expect.The(collection).ToContainExactly("a", "b", "b", "c");
        }

        [Test, Throws]
        public void ToContainExactly_WhenNotAllDuplicatesAreExpected_Throws()
        {
            var collection = new[] { "a", "b", "b", "c", "c" }.AsEnumerable();
            Expect.The(collection).ToContainExactly("a", "b", "b", "c");
        }

        [Test]
        public void ToContainExactly_UsesEqualityComparer()
        {
            var collection = new[] { "a", "b", "c" }.AsEnumerable();
            Expect.The(collection).ToContainExactly(new[] { "C", "B", "A" }, StringComparer.OrdinalIgnoreCase);
        }

        [Test, Throws]
        public void ToContainExactly_WhenContentsDiffer_Throws()
        {
            var collection = new[] { 1, 2, 3, 4 }.AsEnumerable();
            Expect.The(collection).ToContainExactly(2, 3, 4);
        }

        [Test]
        public void ToEnumerateAs_WhenContentIsSame_ReturnsTrue()
        {
            var numberArray = new[] { 2, 4, 6, 8 }.AsEnumerable();
            Expect.The(numberArray).ToEnumerateAs(new List<int> { 2, 4, 6, 8 });
        }

        [Test]
        public void ToEnumerateAs_UsesEqualityComparer()
        {
            var numberArray = new[] { 2, 4, 6, 8 }.AsEnumerable();
            var comparer = new DoublingEqualityComparer();
            Expect.The(numberArray).ToEnumerateAs(new[] { 1, 2, 3, 4 }, comparer);
        }

        [Test, Throws]
        public void ToEnumerateAs_WhenContentDiffers_Throws()
        {
            var numberArray = new[] { 2, 4, 6, 8 }.AsEnumerable();
            Expect.The(numberArray).ToEnumerateAs(new[] { 1, 2, 3, 4 });
        }

        [Test, Throws]
        public void ToEnumerateAs_WhenExpectedHasDifferentLength_Throws()
        {
            var numberArray = new[] { 2, 4, 6, 8 };
            Expect.The(numberArray).ToEnumerateAs(new[] { 2, 4, 6 });
        }

        public class StringLengthEqualityMatcher : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                if (ReferenceEquals(x, y))
                    return true;

                if (ReferenceEquals(x, null) || ReferenceEquals(y, null))
                    return false;

                return x.Length == y.Length;
            }

            public int GetHashCode(string obj)
            {
                if (ReferenceEquals(obj, null))
                    return -1;

                return obj.Length;
            }
        }

        [Test]
        public void ToContain_UsesEqualityMatcher()
        {
            var firstValue = "just a string";
            var secondValue = "don't mind me";
            var comparer = new StringLengthEqualityMatcher();

            Expect.The(new[] { firstValue }.AsEnumerable()).ToContain(secondValue, comparer);
        }

        [Test, Throws]
        public void ToContain_WhenExpectedNotInEnumerable_Throws()
        {
            var collection = new[] { "wtf", "man" }.AsEnumerable();
            Expect.The(collection).ToContain("hi");
        }

        class DoublingEqualityComparer : IEqualityComparer<int>
        {
            public bool Equals(int x, int y)
            {
                if (x < y)
                {
                    return (x * 2) == y;
                }
                else if (x > y)
                {
                    return x == (y * 2);
                }
                else
                {
                    return true;
                }
            }

            public int GetHashCode(int x)
            {
                return x;
            }
        }
    }
}
