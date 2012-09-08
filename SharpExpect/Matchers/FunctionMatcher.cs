using System;

using SharpExpect;

namespace SharpExpect.Matchers
{
	public class FunctionMatcher<T> : BaseMatcher<Func<T>, FunctionMatcher<T>>
	{
		public virtual bool ToThrow<TException>()
			where TException : Exception
		{
			try
			{
				actual();
			}
			catch (TException)
			{
				System.Diagnostics.Debug.WriteLine("caught an expected exception!");
				return true;
			}
			catch (Exception ex)
			{
				// todo(ben): set a meaningful description once non-default descriptions are in
			}

			return false;
		}
	}

	public class ActionMatcher : BaseMatcher<Action, ActionMatcher>
	{
		public virtual bool ToThrow<TException>()
			where TException : Exception
		{
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
				// todo(ben): set a meaningful description once non-default descriptions are in
			}
			
			return false;
		}
	}
}

