using System;

using SharpExpect.Matchers;

namespace SharpExpect.Matchers
{

	public class StringMatcher : BaseObjectMatcher<string, StringMatcher>
	{
		public virtual bool ToBeLongerThan(string expected)
		{
			return actual.Length > expected.Length;
		}

		public virtual bool ToEqual(string expected, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
		{
			return string.Equals(actual, expected, comparison);
		}

		public virtual bool ToContain(string expected, StringComparison comparison = StringComparison.OrdinalIgnoreCase)
		{
			return actual.IndexOf(expected, comparison) >= 0;
		}
	}
	
}
