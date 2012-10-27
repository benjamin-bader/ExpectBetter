using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class CollectionMatcherTests
    {
        [Test]
        public void ToNumber_WhenActualHasSameCountAsExpected_ReturnsTrue()
        {
            var collection = new[] { 1, 2, 3 };
            Expect.The(collection).ToNumber(collection.Length);
        }

        [Test, Throws]
        public void ToNumber_WhenActualHasSmallerCount_Throws()
        {
            var collection = new[] { 1, 2 };
            Expect.The(collection).ToNumber(3);
        }

        [Test, Throws]
        public void ToNumber_WhenActualHasBiggerCount_Throws()
        {
            var collection = new[] { 1, 2, 3, 4 };
            Expect.The(collection).ToNumber(3);
        }

        [Test]
        public void ToNumberAtLeast_WhenActualHasSameCountAsExpected_ReturnsTrue()
        {
            var collection = new[] { 1, 2, 3 };
            Expect.The(collection).ToNumberAtLeast(3);
        }

        [Test]
        public void ToNumberAtLeast_WhenActualHasBiggerCount_ReturnsTrue()
        {
            var collection = new[] { 1, 2, 3, 4 };
            Expect.The(collection).ToNumberAtLeast(3);
        }

        [Test, Throws]
        public void ToNumberAtLeast_WhenActualHasSmallerCount_Throws()
        {
            var collection = new[] { 1, 2 };
            Expect.The(collection).ToNumberAtLeast(3);
        }

        [Test]
        public void ToBeEmpty_WhenActualIsEmpty_ReturnsTrue()
        {
            Expect.The(new object[0]).ToBeEmpty();
        }

        [Test, Throws]
        public void ToBeEmpty_WhenActualIsNotEmpty_Throws()
        {
            Expect.The(new[] { "boom" }).ToBeEmpty();
        }
    }
}
