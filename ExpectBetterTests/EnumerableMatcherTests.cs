using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests
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
