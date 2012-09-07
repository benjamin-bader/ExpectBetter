using System;
using System.Collections.Generic;
using SharpExpect.Matchers;

namespace SharpExpect
{
	public static class Expect
	{
		public static ObjectMatcher The(object actual)
		{
			return ClassWrapper.Wrap<object, ObjectMatcher>(actual);
		}

		public static StringMatcher The(string actual)
		{
			return ClassWrapper.Wrap<string, StringMatcher>(actual);
		}

		public static ComparableMatcher<T> The<T> (IComparable<T> actual)
		{
			return ClassWrapper.Wrap<IComparable<T>, ComparableMatcher<T>>(actual);
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

		public static Int32Matcher The(int actual)
		{
			return ClassWrapper.Wrap<int, Int32Matcher>(actual);
		}

		public static Int64Matcher The(long actual)
		{
			return ClassWrapper.Wrap<long, Int64Matcher>(actual);
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

