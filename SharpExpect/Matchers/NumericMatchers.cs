using System;

using SharpExpect.Matchers;

namespace SharpExpect.Matchers
{
	public class ByteMatcher : BaseMatcher<byte, ByteMatcher>
	{
		public virtual bool ToBeGreaterThan(byte expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(byte expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(byte expected)
		{
			return actual == expected;
		}
	}

	public class SByteMatcher : BaseMatcher<sbyte, SByteMatcher>
	{
		public virtual bool ToBeGreaterThan(sbyte expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(sbyte expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(sbyte expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}
	}

	public class Int16Matcher : BaseMatcher<short, Int16Matcher>
	{
		public virtual bool ToBeGreaterThan(short expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(short expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(short expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}
	}

	public class Int32Matcher : BaseMatcher<int, Int32Matcher>
	{
		public virtual bool ToBeGreaterThan(int expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(int expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(int expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}
	}
	
	public class UInt32Matcher : BaseMatcher<uint, UInt32Matcher>
	{
		public virtual bool ToBeGreaterThan(uint expected)
		{
			return actual > expected;
		}
		
		public virtual bool ToBeLessThan(uint expected)
		{
			return actual < expected;
		}
		
		public virtual bool ToEqual(uint expected)
		{
			return actual == expected;
		}
	}

	public class Int64Matcher : BaseMatcher<long, Int64Matcher>
	{
		public virtual bool ToBeGreaterThan(long expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(long expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(long expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}
	}
	
	public class UInt64Matcher : BaseMatcher<ulong, UInt64Matcher>
	{
		public virtual bool ToBeGreaterThan(ulong expected)
		{
			return actual > expected;
		}
		
		public virtual bool ToBeLessThan(ulong expected)
		{
			return actual < expected;
		}
		
		public virtual bool ToEqual(ulong expected)
		{
			return actual == expected;
		}
	}

	public class SingleMatcher : BaseMatcher<float, SingleMatcher>
	{
		public virtual bool ToBeGreaterThan(float expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(float expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(float expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}

		public virtual bool ToBeWithinDeltaOf(float expected, float delta)
		{
			return Math.Abs(actual - expected) < delta;
		}

		public virtual bool ToBeInfinity()
		{
			return float.IsInfinity(actual);
		}

		public virtual bool ToBePositiveInfinity()
		{
			return float.IsPositiveInfinity(actual);
		}

		public virtual bool ToBeNegativeInfinity()
		{
			return float.IsNegativeInfinity(actual);
		}

		public virtual bool ToBeNaN()
		{
			return float.IsNaN(actual);
		}
	}

	public class DoubleMatcher : BaseMatcher<double, DoubleMatcher>
	{
		public virtual bool ToBeGreaterThan(double expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(double expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(double expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}

		public virtual bool ToBeWithinDeltaOf(double expected, double delta)
		{
			return Math.Abs(actual - expected) < delta;
		}

		public virtual bool ToBeInfinity()
		{
			return double.IsInfinity(actual);
		}

		public virtual bool ToBePositiveInfinity()
		{
			return double.IsPositiveInfinity(actual);
		}

		public virtual bool ToBeNegativeInfinity()
		{
			return double.IsNegativeInfinity(actual);
		}

		public virtual bool ToBeNaN()
		{
			return double.IsNaN(actual);
		}
	}

	public class DecimalMatcher : BaseMatcher<decimal, DecimalMatcher>
	{
		public virtual bool ToBeGreaterThan(decimal expected)
		{
			return actual > expected;
		}

		public virtual bool ToBeLessThan(decimal expected)
		{
			return actual < expected;
		}

		public virtual bool ToEqual(decimal expected)
		{
			return actual == expected;
		}

		public virtual bool ToBePositive()
		{
			return actual >= 0;
		}
	}

	public class IntPtrMatcher : BaseMatcher<IntPtr, IntPtrMatcher>
	{
		public virtual bool ToBeZero()
		{
			return actual == IntPtr.Zero;
		}
	}
	
	public class UIntPtrMatcher : BaseMatcher<UIntPtr, UIntPtrMatcher>
	{
		public virtual bool ToBeZero()
		{
			return actual == UIntPtr.Zero;
		}
	}
}
