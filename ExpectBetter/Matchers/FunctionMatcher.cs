using System;

using ExpectBetter;

namespace ExpectBetter.Matchers
{
	public class FunctionMatcher<T> : BaseMatcher<Func<T>, FunctionMatcher<T>>
	{
		public virtual bool ToThrow<TException>()
			where TException : Exception
		{
			expectedDescription = "an exception of type " + typeof(TException).FullName;

			try
			{
				actual();
			}
			catch (TException)
			{
				return true;
			}
			catch (Exception ex)
			{
				actualDescription = "a function that threw an exception of type " + ex.GetType().FullName;
			}

			return false;
		}
	}

	public class ActionMatcher : BaseMatcher<Action, ActionMatcher>
	{
		public virtual bool ToThrow<TException>()
			where TException : Exception
		{
			expectedDescription = "an exception of type " + typeof(TException).FullName;

			try
			{
				actual();
				actualDescription = "an action that completed successfully";
			}
			catch (TException)
			{
				return true;
			}
			catch (Exception ex)
			{
				actualDescription = "an action that threw an exception of type " + ex.GetType().FullName;
			}
			
			return false;
		}
	}
}

