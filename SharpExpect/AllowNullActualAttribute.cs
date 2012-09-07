using System;

namespace SharpExpect
{
	[AttributeUsage(AttributeTargets.Method, Inherited = true)]
	public class AllowNullActualAttribute : Attribute
	{
	}
}

