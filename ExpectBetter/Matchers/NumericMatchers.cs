using System;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Provides assertions on bytes.
    /// </summary>
    public class ByteMatcher : BaseMatcher<byte, ByteMatcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(byte expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(byte expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(byte expected)
        {
            return actual == expected;
        }
    }

    /// <summary>
    /// Provides assertions on signed bytes.
    /// </summary>
    public class SByteMatcher : BaseMatcher<sbyte, SByteMatcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(sbyte expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(sbyte expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(sbyte expected)
        {
            return actual == expected;
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }
    }

    /// <summary>
    /// Provides assertions on 16-bit integers.
    /// </summary>
    public class Int16Matcher : BaseMatcher<short, Int16Matcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(short expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(short expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(short expected)
        {
            return actual == expected;
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }
    }

    /// <summary>
    /// Provides assertions on unsigned 16-bit integers.
    /// </summary>
    public class UInt16Matcher : BaseMatcher<ushort, UInt16Matcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(ushort expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(ushort expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(ushort expected)
        {
            return actual == expected;
        }
    }

    /// <summary>
    /// Provides assertions on 32-bit integers.
    /// </summary>
    public class Int32Matcher : BaseMatcher<int, Int32Matcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(int expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(int expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(int expected)
        {
            return actual == expected;
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }
    }

    /// <summary>
    /// Provides assertions on unsigned 32-bit integers.
    /// </summary>
    public class UInt32Matcher : BaseMatcher<uint, UInt32Matcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(uint expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(uint expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(uint expected)
        {
            return actual == expected;
        }
    }

    /// <summary>
    /// Provides assertions on 64-bit integers.
    /// </summary>
    public class Int64Matcher : BaseMatcher<long, Int64Matcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(long expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(long expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(long expected)
        {
            return actual == expected;
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }
    }

    /// <summary>
    /// Provides assertions on unsigned 64-bit integers.
    /// </summary>
    public class UInt64Matcher : BaseMatcher<ulong, UInt64Matcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(ulong expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(ulong expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(ulong expected)
        {
            return actual == expected;
        }
    }

    /// <summary>
    /// Provides assertions on IEEE float values.
    /// </summary>
    public class SingleMatcher : BaseMatcher<float, SingleMatcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(float expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(float expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(float expected)
        {
            return actual.Equals(expected);
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }

        /// <summary>
        /// Expect the value to be within a range defined by an
        /// <paramref name="expected"/> value plus or minus a
        /// <paramref name="delta"/> value.
        /// </summary>
        /// <param name="expected">
        /// The center of the range in which the value is expected to be.
        /// </param>
        /// <param name="delta">
        /// A value defining the size of the range in either direction.
        /// </param>
        public virtual bool ToBeWithinDeltaOf(float expected, float delta)
        {
            return Math.Abs(actual - expected) < delta;
        }

        /// <summary>
        /// Expect the value to be no more than <paramref name="maxUlps"/>
        /// units of least precision (smallest representable increments) away
        /// from <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">
        /// The center of the range in which the value is expected to be.
        /// </param>
        /// <param name="maxUlps">
        /// The maximum number of ULP's the value is allowed to deviate from
        /// <paramref name="expected"/>.
        /// </param>
        /// <seealso href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition"/>
        public virtual bool ToBeWithinUlpsOf(float expected, int maxUlps = 4)
        {
            // Check for exact equality
            if (actual == expected) return true;
            // Check for different sign
            if (Math.Sign(actual) != Math.Sign(expected)) return false;
            // Obtain integer representation
            byte[] actualBytes = BitConverter.GetBytes(actual);
            byte[] expectedBytes = BitConverter.GetBytes(expected);
            int actualInt = BitConverter.ToInt32(actualBytes, 0);
            int expectedInt = BitConverter.ToInt32(expectedBytes, 0);
            // Check ULP difference
            return Math.Abs(actualInt - expectedInt) < maxUlps;
        }

        /// <summary>
        /// Expect the value to be a representation of infinity.
        /// </summary>
        public virtual bool ToBeInfinity()
        {
            return float.IsInfinity(actual);
        }

        /// <summary>
        /// Expect the value to be a representation of positive infinity.
        /// </summary>
        public virtual bool ToBePositiveInfinity()
        {
            return float.IsPositiveInfinity(actual);
        }

        /// <summary>
        /// Expect the value to be a representation of negative infinity.
        /// </summary>
        public virtual bool ToBeNegativeInfinity()
        {
            return float.IsNegativeInfinity(actual);
        }

        /// <summary>
        /// Expect the value to be the IEEE not-a-number value.
        /// </summary>
        public virtual bool ToBeNaN()
        {
            return float.IsNaN(actual);
        }
    }

    /// <summary>
    /// Provides assertions on IEEE double values.
    /// </summary>
    public class DoubleMatcher : BaseMatcher<double, DoubleMatcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(double expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(double expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(double expected)
        {
            return actual.Equals(expected);
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }

        /// <summary>
        /// Expect the value to be within a range defined by an
        /// <paramref name="expected"/> value plus or minus a
        /// <paramref name="delta"/> value.
        /// </summary>
        /// <param name="expected">
        /// The center of the range in which the value is expected to be.
        /// </param>
        /// <param name="delta">
        /// A value defining the size of the range in either direction.
        /// </param>
        public virtual bool ToBeWithinDeltaOf(double expected, double delta)
        {
            return Math.Abs(actual - expected) < delta;
        }

        /// <summary>
        /// Expect the value to be no more than <paramref name="maxUlps"/>
        /// units of least precision (smallest representable increments) away
        /// from <paramref name="expected"/>.
        /// </summary>
        /// <param name="expected">
        /// The center of the range in which the value is expected to be.
        /// </param>
        /// <param name="maxUlps">
        /// The maximum number of ULP's the value is allowed to deviate from
        /// <paramref name="expected"/>.
        /// </param>
        /// <seealso href="http://randomascii.wordpress.com/2012/02/25/comparing-floating-point-numbers-2012-edition"/>
        public virtual bool ToBeWithinUlpsOf(double expected, long maxUlps = 4)
        {
            // Check for exact equality
            if (actual == expected) return true;
            // Check for different sign
            if (Math.Sign(actual) != Math.Sign(expected)) return false;
            // Obtain integer representation
            byte[] actualBytes = BitConverter.GetBytes(actual);
            byte[] expectedBytes = BitConverter.GetBytes(expected);
            long actualInt = BitConverter.ToInt64(actualBytes, 0);
            long expectedInt = BitConverter.ToInt64(expectedBytes, 0);
            // Check ULP difference
            return Math.Abs(actualInt - expectedInt) < maxUlps;
        }

        /// <summary>
        /// Expect the value to be a representation of infinity.
        /// </summary>
        public virtual bool ToBeInfinity()
        {
            return double.IsInfinity(actual);
        }

        /// <summary>
        /// Expect the value to be a representation of positive infinity.
        /// </summary>
        public virtual bool ToBePositiveInfinity()
        {
            return double.IsPositiveInfinity(actual);
        }

        /// <summary>
        /// Expect the value to be a representation of negative infinity.
        /// </summary>
        public virtual bool ToBeNegativeInfinity()
        {
            return double.IsNegativeInfinity(actual);
        }

        /// <summary>
        /// Expect the value to be the IEEE not-a-number value.
        /// </summary>
        public virtual bool ToBeNaN()
        {
            return double.IsNaN(actual);
        }
    }

    /// <summary>
    /// Provides assertions on <see cref="System.Decimal"/> values.
    /// </summary>
    public class DecimalMatcher : BaseMatcher<decimal, DecimalMatcher>
    {
        /// <summary>
        /// Expect the value to be greater than the given value.
        /// </summary>
        public virtual bool ToBeGreaterThan(decimal expected)
        {
            return actual > expected;
        }

        /// <summary>
        /// Expect the value to be less than the given value.
        /// </summary>
        public virtual bool ToBeLessThan(decimal expected)
        {
            return actual < expected;
        }

        /// <summary>
        /// Expect the value to equal the given value.
        /// </summary>
        public virtual bool ToEqual(decimal expected)
        {
            return actual == expected;
        }

        /// <summary>
        /// Expect the value to be positive, i.e greater than or equal to zero.
        /// </summary>
        public virtual bool ToBePositive()
        {
            return actual >= 0;
        }
    }

    /// <summary>
    /// Provides assertions on <see cref="IntPtr"/> values.
    /// </summary>
    public class IntPtrMatcher : BaseMatcher<IntPtr, IntPtrMatcher>
    {
        /// <summary>
        /// Expect the pointer value to be zero.
        /// </summary>
        public virtual bool ToBeZero()
        {
            return actual == IntPtr.Zero;
        }
    }

    /// <summary>
    /// Provides assertions on <see cref="UIntPtr"/> values.
    /// </summary>
    public class UIntPtrMatcher : BaseMatcher<UIntPtr, UIntPtrMatcher>
    {
        /// <summary>
        /// Expect the pointer value to be zero.
        /// </summary>
        public virtual bool ToBeZero()
        {
            return actual == UIntPtr.Zero;
        }
    }
}
