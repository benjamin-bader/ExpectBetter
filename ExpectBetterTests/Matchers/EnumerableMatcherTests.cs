using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            Expect.The(collection).ToContainInOrder(new[] { 2, 4, 6 });
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToContainInOrder_WhenNotContainedAtAll_Throws()
        {
            var collection = new[] { 1, 2, 3, 4, 5 }.AsEnumerable();
            Expect.The(collection).ToContainInOrder(new[] { 7 });
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToContainInOrder_WhenContained_InWrongOrder_Throws()
        {
            var collection = new[] { 2, 3, 4 }.AsEnumerable();
            Expect.The(collection).ToContainInOrder(new[] { 4, 2 });
        }

        [Test]
        public void ToContainExactly_WhenContentsMatch_ReturnsTrue()
        {
            var collection = new[] { "a", "b", "c" }.AsEnumerable();
            var items = new List<string>();

            items.Add("a");
            items.Add("b");
            items.Add("c");

            Expect.The(collection).ToContainExactly(items);
        }

        [Test]
        public void ToContainExactly_WhenContentsOutOfOrder_ReturnsTrue()
        {
            var collection = new[] { "a", "b", "c" }.AsEnumerable();
            var items = new List<string>();

            items.Add("a");
            items.Add("c");
            items.Add("b");

            Expect.The(collection).ToContainExactly(items);
        }

        [Test]
        public void ToContainExactly_AccountForDuplicates()
        {
            var collection = new[] { "a", "b", "b", "c" }.AsEnumerable();
            Expect.The(collection).ToContainExactly(new[] { "a", "b", "b", "c" });
        }

        [Test]
        public void ToContainExactly_UsesEqualityComparer()
        {
            var collection = new[] { "a", "b", "c" }.AsEnumerable();
            Expect.The(collection).ToContainExactly(new[] { "C", "B", "A" }, StringComparer.OrdinalIgnoreCase);
        }

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToContainExactly_WhenContentsDiffer_Throws()
        {
            var collection = new[] { 1, 2, 3, 4 }.AsEnumerable();
            Expect.The(collection).ToContainExactly(new[] { 2, 3, 4 });
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

        [Test, ExpectedException(typeof(ExpectationException))]
        public void ToContain_WhenExpectedNotInEnumerable_Throws()
        {
            var collection = new[] { "wtf", "man" }.AsEnumerable();
            Expect.The(collection).ToContain("hi");
        }
    }
}
