using System;
using System.Collections.Generic;

using ExpectBetter.Matchers;

namespace ExpectBetter
{
    /// <summary>
    /// Provides matchers with which to state expectations.
    /// </summary>
    public class Expect
    {
        /// <summary>
        /// Default constructor.  Present to prevent instantiation of this class.
        /// </summary>
        protected Expect()
        {

        }

        /// <summary>
        /// Set expectations for an <see cref="Object"/>.
        /// </summary>
        public static ObjectMatcher The(object actual)
        {
            return Expectations.Wrap<object, ObjectMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Char"/>;
        /// </summary>
        public static CharMatcher The(char actual)
        {
            return Expectations.Wrap<char, CharMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="IEquatable&lt;T&gt;"/>.
        /// </summary>
        public static EquatableMatcher<T> The<T>(IEquatable<T> actual)
        {
            return Expectations.Wrap<IEquatable<T>, EquatableMatcher<T>>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Nullable&lt;T&gt;"/>
        /// </summary>
        public static NullableMatcher<T> The<T>(T? actual)
            where T : struct
        {
            return Expectations.Wrap<T?, NullableMatcher<T>>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="String"/>
        /// </summary>
        public static StringMatcher The(string actual)
        {
            return Expectations.Wrap<string, StringMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="IComparable&lt;T&gt;"/>.
        /// </summary>
        public static ComparableMatcher<T> The<T>(IComparable<T> actual)
        {
            return Expectations.Wrap<IComparable<T>, ComparableMatcher<T>>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="IEnumerable&lt;T&gt;"/>.
        /// </summary>
        public static EnumerableMatcher<T> The<T>(IEnumerable<T> actual)
        {
            return Expectations.Wrap<IEnumerable<T>, EnumerableMatcher<T>>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="IEnumerable&lt;T&gt;"/>.
        /// </summary>
        public static CollectionMatcher<T> The<T>(ICollection<T> actual)
        {
            return Expectations.Wrap<ICollection<T>, CollectionMatcher<T>>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="IDictionary&lt;TKey, TValue&gt;"/>.
        /// </summary>
        public static DictionaryMatcher<TKey, TValue> The<TKey, TValue>(IDictionary<TKey, TValue> actual)
        {
            return Expectations.Wrap<IDictionary<TKey, TValue>, DictionaryMatcher<TKey, TValue>>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Boolean"/> value.
        /// </summary>
        public static BoolMatcher The(bool actual)
        {
            return Expectations.Wrap<bool, BoolMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Byte"/>.
        /// </summary>
        public static ByteMatcher The(byte actual)
        {
            return Expectations.Wrap<byte, ByteMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="SByte"/>.
        /// </summary>
        public static SByteMatcher The(sbyte actual)
        {
            return Expectations.Wrap<sbyte, SByteMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="Int16"/>.
        /// </summary>
        public static Int16Matcher The(short actual)
        {
            return Expectations.Wrap<short, Int16Matcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="UInt16"/>.
        /// </summary>
        public static UInt16Matcher The(ushort actual)
        {
            return Expectations.Wrap<ushort, UInt16Matcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="Int32"/>
        /// </summary>
        public static Int32Matcher The(int actual)
        {
            return Expectations.Wrap<int, Int32Matcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="UInt32"/>.
        /// </summary>
        public static UInt32Matcher The(uint actual)
        {
            return Expectations.Wrap<uint, UInt32Matcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="Int64"/>.
        /// </summary>
        public static Int64Matcher The(long actual)
        {
            return Expectations.Wrap<long, Int64Matcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="UInt64"/>.
        /// </summary>
        public static UInt64Matcher The(ulong actual)
        {
            return Expectations.Wrap<ulong, UInt64Matcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Single"/>.
        /// </summary>
        public static SingleMatcher The(float actual)
        {
            return Expectations.Wrap<float, SingleMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Double"/>.
        /// </summary>
        public static DoubleMatcher The(double actual)
        {
            return Expectations.Wrap<double, DoubleMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Decimal"/>.
        /// </summary>
        public static DecimalMatcher The(decimal actual)
        {
            return Expectations.Wrap<decimal, DecimalMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="UIntPtr"/>.
        /// </summary>
        public static UIntPtrMatcher The(UIntPtr actual)
        {
            return Expectations.Wrap<UIntPtr, UIntPtrMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="IntPtr"/>.
        /// </summary>
        public static IntPtrMatcher The(IntPtr actual)
        {
            return Expectations.Wrap<IntPtr, IntPtrMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="DateTime"/>.
        /// </summary>
        public static DateTimeMatcher The(DateTime actual)
        {
            return Expectations.Wrap<DateTime, DateTimeMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for an <see cref="Action"/>.
        /// </summary>
        public static ActionMatcher The(Action actual)
        {
            return Expectations.Wrap<Action, ActionMatcher>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Func&lt;T&gt;"/>.
        /// </summary>
        public static FunctionMatcher<T> The<T>(Func<T> actual)
        {
            return Expectations.Wrap<Func<T>, FunctionMatcher<T>>(actual);
        }

        /// <summary>
        /// Set expectations for a <see cref="Type"/>.
        /// </summary>
        public static TypeMatcher The(Type actual)
        {
            return Expectations.Wrap<Type, TypeMatcher>(actual);
        }
    }
}

