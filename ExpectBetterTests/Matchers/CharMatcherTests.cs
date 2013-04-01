using ExpectBetter;
using NUnit.Framework;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class CharMatcherTests
    {
        [Test]
        public void ToBeUpperCase_WhenActualIsUpper_Passes()
        {
            Expect.The('A').ToBeUpperCase();
        }

        [Test, Throws]
        public void ToBeUpperCase_WhenActualIsLower_Throws()
        {
            Expect.The('a').ToBeUpperCase();
        }

        [Test, Throws]
        public void ToBeUpperCase_WhenActualIsWhitespace_Throws()
        {
            Expect.The(' ').ToBeUpperCase();
        }

        [Test, Throws]
        public void ToBeUpperCase_WhenActualIsPunctuation_Throws()
        {
            Expect.The('.').ToBeUpperCase();
        }

        [Test, Throws]
        public void ToBeLowerCase_WhenActualIsUpper_Throws()
        {
            Expect.The('A').ToBeLowerCase();
        }

        [Test]
        public void ToBeLowerCase_WhenActualIsLower_Passes()
        {
            Expect.The('a').ToBeLowerCase();
        }

        [Test, Throws]
        public void ToBeLowerCase_WhenActualIsWhitespace_Throws()
        {
            Expect.The(' ').ToBeLowerCase();
        }

        [Test, Throws]
        public void ToBeLowerCase_WhenActualIsPunctuation_Throws()
        {
            Expect.The('.').ToBeLowerCase();
        }

        [Test]
        public void ToEqual_WhenActualIsExpected_Passes()
        {
            Expect.The('.').ToEqual('.');
        }

        [Test, Throws]
        public void ToEqual_WhenActualDiffers_Throws()
        {
            Expect.The(',').ToEqual('.');
        }
    }
}
