using System;

namespace SharpExpect
{
	public class ExpectationException : ApplicationException
	{
		public ExpectationException(string message)
			: base(message)
		{
		}
	}
}

