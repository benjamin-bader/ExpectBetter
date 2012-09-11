using System;

namespace SharpExpect.Matchers
{

	public class BaseObjectMatcher<T, M> : BaseMatcher<T, M>
		where M : BaseObjectMatcher<T, M>
	{
		[AllowNullActual]
		public virtual bool ToBeNull()
		{
			return ReferenceEquals(null, actual);
		}

		[AllowNullActual]
		public virtual bool ToBeTheSameAs(T expected)
		{
			return ReferenceEquals(actual, expected);
		}

		public virtual bool ToEqual(T expected)
		{
			return actual.Equals(expected);
		}

		public virtual bool ToBeAnInstanceOf<TExpected>()
		{
			expectedDescription = typeof(TExpected).FullName;
			return typeof(TExpected).IsAssignableFrom(actual.GetType());
		}
	}
	
}
