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
		// Initially this was ToThrow<TException>() where TException : Exception.
		// Under mono 2.10.9 x86 with 2.10.9.0 of mcs, at least, this construct
		// seemed to result in all exceptions being caught, and not just the specific
		// TException.  Hence, this ugliness.

		public virtual bool ToThrow(Type exception)
		{
			try
			{
				actual();
			}
			catch (Exception ex)
			{
				if (exception.IsAssignableFrom(ex.GetType()))
				{
					// a 'catch (TException)' block would be so much nicer - oh well.
					System.Diagnostics.Debug.WriteLine("caught an expected exception!");
					return true;
				}

				// todo(ben): set a meaningful description once non-default descriptions are in
			}
			
			return false;
		}
	}
}

