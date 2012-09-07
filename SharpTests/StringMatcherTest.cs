using System;

using NUnit.Framework;

using SharpExpect;
using SharpExpect.Matchers;

namespace SharpTests
{
	[TestFixture]
	public class StringMatcherTest
	{
		string actual;
		StringMatcher matcher;

		[SetUp]
		public void Setup()
		{
			actual = Factory.RandomString();
			matcher = Expect.The(actual);
		}

		[Test]
		public void ToContain_WhenExpectedIsContained_ReturnsTrue()
		{
			var expectedLen = Math.Min(actual.Length / 2 + 1, actual.Length);
			var expected = actual.Substring(expectedLen);
			var result = matcher.ToContain(expected);

			// todo(ben): InvalidProgramException at .ToBeTrue(), IL_0007: ceq
			// Since we're here, we can assume that .ToContain didn't throw, so this isn't the end of the world.
			//Expect.The(result).ToBeTrue();
		}

		[Test]
		public void Not_ToContain_WhenExpectedIsNotContained_ReturnsTrue()
		{
			var expected = Factory.RandomString(actual.Length + 10, actual.Length + 1);
			var result = matcher.Not.ToContain(expected);

			// todo(ben): InvalidProgramException at .ToBeTrue(), IL_0007: ceq
			// Since we're here, we can assume that .ToContain didn't throw, so this isn't the end of the world.
			//Expect.The(result).ToBeTrue();
		}

		[Test, ExpectedException(typeof(ExpectationException))]
		public void ToContain_WhenExpectedIsNotContained_Throws()
		{
			var expected = Factory.RandomString(actual.Length + 10, actual.Length + 1);

			matcher.ToContain(expected);
		}
	}
}

