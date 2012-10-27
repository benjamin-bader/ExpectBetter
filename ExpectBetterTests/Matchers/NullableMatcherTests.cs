using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class NullableMatcherTests
    {
        [Test]
        public void ToBeNull_WhenActualIsNull_ReturnsTrue()
        {
            var actual = (int?) null;
            Expect.The(actual).ToBeNull();
        }

        [Test, Throws]
        public void ToBeNull_WhenActualHasValue_Throws()
        {
            var actual = 1 as int?;
            Expect.The(actual).ToBeNull();
        }

        [Test]
        public void ToHaveAValue_WhenActualHasValue_ReturnsTrue()
        {
            var actual = 1 as int?;
            Expect.The(actual).ToHaveAValue();
        }

        [Test, Throws]
        public void ToHaveAValue_WhenActualIsNull_Throws()
        {
            var actual = null as byte?;
            Expect.The(actual).ToHaveAValue();
        }
    }
}