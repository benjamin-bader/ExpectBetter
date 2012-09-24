using System;

namespace ExpectBetter
{
    /// <summary>
    /// Indicates that a value of <see langword="null"/> is acceptable for the
    /// decorated matcher method.
    /// </summary>
    [AttributeUsage(AttributeTargets.Method, Inherited = true)]
    public class AllowNullActualAttribute : Attribute
    {
    }
}

