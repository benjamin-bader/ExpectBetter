using System;

using ExpectBetter.Matchers;

namespace ExpectBetter.Matchers
{
	public class ComparableMatcher<T> : BaseObjectMatcher<IComparable<T>, ComparableMatcher<T>>
	{
		public virtual bool ToEqual(T expected)
		{
			return actual.CompareTo(expected) == 0;
		}

		public virtual bool ToBeLessThan(T expected)
		{
			return actual.CompareTo(expected) < 0;
		}

		public virtual bool ToBeGreaterThan(T expected)
		{
			return actual.CompareTo(expected) > 0;
		}
	}
}

