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

			Expect.The(Expect.The(b)).ToBeAnInstanceOf(typeof(ByteMatcher));
			Expect.The(Expect.The(sb)).ToBeAnInstanceOf(typeof(SByteMatcher));
			Expect.The(Expect.The(s)).ToBeAnInstanceOf(typeof(Int16Matcher));
			Expect.The(Expect.The(us)).ToBeAnInstanceOf(typeof(UInt16Matcher));
			Expect.The(Expect.The(i)).ToBeAnInstanceOf(typeof(Int32Matcher));
			Expect.The(Expect.The(ui)).ToBeAnInstanceOf(typeof(UInt32Matcher));
			Expect.The(Expect.The(l)).ToBeAnInstanceOf(typeof(Int64Matcher));
			Expect.The(Expect.The(ul)).ToBeAnInstanceOf(typeof(UInt64Matcher));
			Expect.The(Expect.The(f)).ToBeAnInstanceOf(typeof(SingleMatcher));
			Expect.The(Expect.The(d)).ToBeAnInstanceOf(typeof(DoubleMatcher));
			Expect.The(Expect.The(m)).ToBeAnInstanceOf(typeof(DecimalMatcher));
			Expect.The(Expect.The(ip)).ToBeAnInstanceOf(typeof(IntPtrMatcher));
			Expect.The(Expect.The(uip)).ToBeAnInstanceOf(typeof(UIntPtrMatcher));
		}
	}
}

