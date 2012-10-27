using System;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests
{
    /// <summary>
    /// Shorthand for [ExpectedException(typeof(ExpectationException))].
    /// </summary>
    [AttributeUsage(AttributeTargets.Method)]
    public class ThrowsAttribute : ExpectedExceptionAttribute
    {
        public ThrowsAttribute()
            : base(typeof(ExpectationException))
        {
        }
    }
}
