using System;
using NUnit.Framework;

using SharpExpect;
using SharpExpect.Matchers;

namespace SharpTests
{
	[TestFixture]
	public class ExpectTests
	{
		[Test]
		public void Expect_The_CanDistinguishValueTypes()
		{
			byte b = 0;
			sbyte sb = 0;
			short s = 0;
			int i = 0;
			long l = 0;
			float f = 0.0f;
			double d = 0.0d;
			decimal m = 0.0M;

			Expect.The(Expect.The(b)).ToBeAnInstanceOf(typeof(ByteMatcher));
			Expect.The(Expect.The(sb)).ToBeAnInstanceOf(typeof(SByteMatcher));
			Expect.The(Expect.The(s)).ToBeAnInstanceOf(typeof(Int16Matcher));
			Expect.The(Expect.The(i)).ToBeAnInstanceOf(typeof(Int32Matcher));
			Expect.The(Expect.The(l)).ToBeAnInstanceOf(typeof(Int64Matcher));
			Expect.The(Expect.The(f)).ToBeAnInstanceOf(typeof(SingleMatcher));
			Expect.The(Expect.The(d)).ToBeAnInstanceOf(typeof(DoubleMatcher));
			Expect.The(Expect.The(m)).ToBeAnInstanceOf(typeof(DecimalMatcher));

			Expect.The(() => { throw new NotImplementedException(); }).ToThrow(typeof(InvalidCastException));
		}
	}
}

