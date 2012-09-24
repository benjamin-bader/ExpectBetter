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
            return ClassWrapper.Wrap<object, ObjectMatcher>(actual);
        }

        public static StringMatcher The(string actual)
        {
            return ClassWrapper.Wrap<string, StringMatcher>(actual);
        }

        public static ComparableMatcher<T> The<T>(IComparable<T> actual)
        {
            return ClassWrapper.Wrap<IComparable<T>, ComparableMatcher<T>>(actual);
        }

        public static EnumerableMatcher<T> The<T>(IEnumerable<T> actual)
        {
            return ClassWrapper.Wrap<IEnumerable<T>, EnumerableMatcher<T>>(actual);
        }

        public static CollectionMatcher<T> The<T>(ICollection<T> actual)
        {
            return ClassWrapper.Wrap<ICollection<T>, CollectionMatcher<T>>(actual);
        }

        public static DictionaryMatcher<K, V> The<K, V>(IDictionary<K, V> actual)
        {
            return ClassWrapper.Wrap<IDictionary<K, V>, DictionaryMatcher<K, V>>(actual);
        }

        public static BoolMatcher The(bool actual)
        {
            return ClassWrapper.Wrap<bool, BoolMatcher>(actual);
        }

        public static ByteMatcher The(byte actual)
        {
            return ClassWrapper.Wrap<byte, ByteMatcher>(actual);
        }

        public static SByteMatcher The(sbyte actual)
        {
            return ClassWrapper.Wrap<sbyte, SByteMatcher>(actual);
        }

        public static Int16Matcher The(short actual)
        {
            return ClassWrapper.Wrap<short, Int16Matcher>(actual);
        }

        public static UInt16Matcher The(ushort actual)
        {
            return ClassWrapper.Wrap<ushort, UInt16Matcher>(actual);
        }

        public static Int32Matcher The(int actual)
        {
            return ClassWrapper.Wrap<int, Int32Matcher>(actual);
        }

        public static UInt32Matcher The(uint actual)
        {
            return ClassWrapper.Wrap<uint, UInt32Matcher>(actual);
        }

        public static Int64Matcher The(long actual)
        {
            return ClassWrapper.Wrap<long, Int64Matcher>(actual);
        }

        public static UInt64Matcher The(ulong actual)
        {
            return ClassWrapper.Wrap<ulong, UInt64Matcher>(actual);
        }

        public static SingleMatcher The(float actual)
        {
            return ClassWrapper.Wrap<float, SingleMatcher>(actual);
        }

        public static DoubleMatcher The(double actual)
        {
            return ClassWrapper.Wrap<double, DoubleMatcher>(actual);
        }

        public static DecimalMatcher The(decimal actual)
        {
            return ClassWrapper.Wrap<decimal, DecimalMatcher>(actual);
        }

        public static UIntPtrMatcher The(UIntPtr actual)
        {
            return ClassWrapper.Wrap<UIntPtr, UIntPtrMatcher>(actual);
        }

        public static IntPtrMatcher The(IntPtr actual)
        {
            return ClassWrapper.Wrap<IntPtr, IntPtrMatcher>(actual);
        }

        public static DateTimeMatcher The(DateTime actual)
        {
            return ClassWrapper.Wrap<DateTime, DateTimeMatcher>(actual);
        }

        public static ActionMatcher The(Action actual)
        {
            return ClassWrapper.Wrap<Action, ActionMatcher>(actual);
        }

        public static FunctionMatcher<T> The<T>(Func<T> actual)
        {
            return ClassWrapper.Wrap<Func<T>, FunctionMatcher<T>>(actual);
        }
    }
}

