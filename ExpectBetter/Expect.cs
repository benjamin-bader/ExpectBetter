using System;
using System.Collections.Generic;

using ExpectBetter.Matchers;

namespace ExpectBetter
{
    public class Expect
    {
        protected Expect()
        {

        }

        public static ObjectMatcher The(object actual)
        {
            return Expectations.Wrap<object, ObjectMatcher>(actual);
        }

        public static StringMatcher The(string actual)
        {
            return Expectations.Wrap<string, StringMatcher>(actual);
        }

        public static ComparableMatcher<T> The<T>(IComparable<T> actual)
        {
            return Expectations.Wrap<IComparable<T>, ComparableMatcher<T>>(actual);
        }

        public static EnumerableMatcher<T> The<T>(IEnumerable<T> actual)
        {
            return Expectations.Wrap<IEnumerable<T>, EnumerableMatcher<T>>(actual);
        }

        public static CollectionMatcher<T> The<T>(ICollection<T> actual)
        {
            return Expectations.Wrap<ICollection<T>, CollectionMatcher<T>>(actual);
        }

        public static DictionaryMatcher<K, V> The<K, V>(IDictionary<K, V> actual)
        {
            return Expectations.Wrap<IDictionary<K, V>, DictionaryMatcher<K, V>>(actual);
        }

        public static BoolMatcher The(bool actual)
        {
            return Expectations.Wrap<bool, BoolMatcher>(actual);
        }

        public static ByteMatcher The(byte actual)
        {
            return Expectations.Wrap<byte, ByteMatcher>(actual);
        }

        public static SByteMatcher The(sbyte actual)
        {
            return Expectations.Wrap<sbyte, SByteMatcher>(actual);
        }

        public static Int16Matcher The(short actual)
        {
            return Expectations.Wrap<short, Int16Matcher>(actual);
        }

        public static UInt16Matcher The(ushort actual)
        {
            return Expectations.Wrap<ushort, UInt16Matcher>(actual);
        }

        public static Int32Matcher The(int actual)
        {
            return Expectations.Wrap<int, Int32Matcher>(actual);
        }

        public static UInt32Matcher The(uint actual)
        {
            return Expectations.Wrap<uint, UInt32Matcher>(actual);
        }

        public static Int64Matcher The(long actual)
        {
            return Expectations.Wrap<long, Int64Matcher>(actual);
        }

        public static UInt64Matcher The(ulong actual)
        {
            return Expectations.Wrap<ulong, UInt64Matcher>(actual);
        }

        public static SingleMatcher The(float actual)
        {
            return Expectations.Wrap<float, SingleMatcher>(actual);
        }

        public static DoubleMatcher The(double actual)
        {
            return Expectations.Wrap<double, DoubleMatcher>(actual);
        }

        public static DecimalMatcher The(decimal actual)
        {
            return Expectations.Wrap<decimal, DecimalMatcher>(actual);
        }

        public static UIntPtrMatcher The(UIntPtr actual)
        {
            return Expectations.Wrap<UIntPtr, UIntPtrMatcher>(actual);
        }

        public static IntPtrMatcher The(IntPtr actual)
        {
            return Expectations.Wrap<IntPtr, IntPtrMatcher>(actual);
        }

        public static DateTimeMatcher The(DateTime actual)
        {
            return Expectations.Wrap<DateTime, DateTimeMatcher>(actual);
        }

        public static ActionMatcher The(Action actual)
        {
            return Expectations.Wrap<Action, ActionMatcher>(actual);
        }

        public static FunctionMatcher<T> The<T>(Func<T> actual)
        {
            return Expectations.Wrap<Func<T>, FunctionMatcher<T>>(actual);
        }
    }
}

