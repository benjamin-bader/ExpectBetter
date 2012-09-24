using System;
using NUnit.Framework;

using ExpectBetter;
using ExpectBetter.Matchers;

namespace ExpectBetterTests
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
			ushort us = 0;
			int i = 0;
			uint ui = 0u;
			long l = 0;
			ulong ul = 0ul;
			float f = 0.0f;
			double d = 0.0d;
			decimal m = 0.0M;
			IntPtr ip = IntPtr.Zero;
			UIntPtr uip = UIntPtr.Zero;

			Expect.The(Expect.The(b)).ToBeAnInstanceOf<ByteMatcher>();
			Expect.The(Expect.The(sb)).ToBeAnInstanceOf<SByteMatcher>();
			Expect.The(Expect.The(s)).ToBeAnInstanceOf<Int16Matcher>();
			Expect.The(Expect.The(us)).ToBeAnInstanceOf<UInt16Matcher>();
			Expect.The(Expect.The(i)).ToBeAnInstanceOf<Int32Matcher>();
			Expect.The(Expect.The(ui)).ToBeAnInstanceOf<UInt32Matcher>();
			Expect.The(Expect.The(l)).ToBeAnInstanceOf<Int64Matcher>();
			Expect.The(Expect.The(ul)).ToBeAnInstanceOf<UInt64Matcher>();
			Expect.The(Expect.The(f)).ToBeAnInstanceOf<SingleMatcher>();
			Expect.The(Expect.The(d)).ToBeAnInstanceOf<DoubleMatcher>();
			Expect.The(Expect.The(m)).ToBeAnInstanceOf<DecimalMatcher>();
			Expect.The(Expect.The(ip)).ToBeAnInstanceOf<IntPtrMatcher>();
			Expect.The(Expect.The(uip)).ToBeAnInstanceOf<UIntPtrMatcher>();
		}
	}
}

