using System;

namespace ExpectBetter
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class AllowNullActualAttribute : Attribute
	{
	}
}

