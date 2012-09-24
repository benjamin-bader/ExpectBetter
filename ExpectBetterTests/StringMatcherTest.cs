using System;

using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Matchers;

namespace ExpectBetterTests
{
	[TestFixture]
	public class StringMatcherTest
	{
		string actual;
		StringMatcher matcher;

		[SetUp]
		public void Setup()
		{
			actual = Factory.RandomString(20, 5);
			matcher = Expect.The(actual);
		}

		[Test]
		public void ToContain_WhenExpectedIsContained_ReturnsTrue()
		{
			var expectedLen = Math.Min(actual.Length / 2 + 1, actual.Length);
			var expected = actual.Substring(expectedLen);
			var result = matcher.ToContain(expected);

			// todo(ben): InvalidProgramException at .ToBeTrue(), IL_002d: call 0x0a000006
			// This is the issue currently plaguing value type matchers - monodis chokes on
			// generated constructors before ever getting to 
			// Since we're here, we can assume that .ToContain didn't throw, so this isn't the end of the world.
			// Expect.The(result).ToBeTrue();
			Assert.IsTrue(result);
		}

		[Test]
		public void Not_ToContain_WhenExpectedIsNotContained_ReturnsTrue()
		{
			var expected = Factory.RandomString(actual.Length + 10, actual.Length + 1);
			var result = matcher.Not.ToContain(expected);

            Expect.The(result).ToBeTrue();
		}

		[Test, ExpectedException(typeof(ExpectationException))]
		public void ToContain_WhenExpectedIsNotContained_Throws()
		{
			var expected = Factory.RandomString(actual.Length + 10, actual.Length + 1);

			matcher.ToContain(expected);
		}

		[Test]
		public void ToBeLongerThan_WhenExpectedIsShorter_ReturnsTrue()
		{
			var expected = Factory.RandomString(actual.Length - 1);
			var result = matcher.ToBeLongerThan(expected);

			Assert.IsTrue(result);
		}

		[Test, ExpectedException(typeof(ExpectationException))]
		public void ToBeLongerThan_WhenExpectedIsLonger_Throws()
		{
			var expected = Factory.RandomString(actual.Length * 2, actual.Length + 1);
			matcher.ToBeLongerThan(expected);
		}
		
		[Test, ExpectedException(typeof(ExpectationException))]
		public void Not_ToBeLongerThan_WhenExpectedIsShorter_Throws()
		{
			var expected = Factory.RandomString(actual.Length - 1);
			matcher.Not.ToBeLongerThan(expected);
		}
		
		[Test]
		public void ToBeLongerThan_WhenExpectedIsLonger_ReturnsTrue()
		{
			var expected = Factory.RandomString(actual.Length * 2, actual.Length + 1);
			var result = matcher.Not.ToBeLongerThan(expected);

			Assert.IsTrue(result);
		}
	}
}

