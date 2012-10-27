using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class BoolMatcherTests
    {
        [Test]
        public void ToBeTrue_WhenTrue_ReturnsTrue()
        {
            Expect.The(true).ToBeTrue();
        }

        [Test, Throws]
        public void ToBeTrue_WhenFalse_Throws()
        {
            Expect.The(false).ToBeTrue();
        }

        [Test]
        public void ToBeFalse_WhenFalse_ReturnsTrue()
        {
            Expect.The(false).ToBeFalse();
        }

        [Test, Throws]
        public void ToBeFalse_WhenTrue_Throws()
        {
            Expect.The(true).ToBeFalse();
        }

        [Test]
        public void ToEqual_WhenExpectedEqualsActual_ReturnsTrue()
        {
            Expect.The(true).ToEqual(true);
        }

        [Test, Throws]
        public void ToEqual_WhenExpectedDiffersFromActual_Throws()
        {
            Expect.The(true).ToEqual(false);
        }
    }
}
