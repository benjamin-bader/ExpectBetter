using System;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class EquatableMatcherTests
    {
        private class OddnessEquatable : IEquatable<int>
        {
            public bool Equals(int other)
            {
                return (other & 1) == 1;
            }
        }
            
        [Test]
        public void ToEqual_WhenObjectsAreEqual_ReturnsTrue()
        {
            var equatable = new OddnessEquatable();
            Expect.The(equatable).ToEqual(43);
            Expect.The(equatable).ToEqual(111);
        }

        [Test, Throws]
        public void ToEqual_WhenObjectsAreNotEqual_Throws()
        {
            Expect.The(new OddnessEquatable()).ToEqual(2);
        }
    }
}
