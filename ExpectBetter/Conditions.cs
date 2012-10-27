using System;

namespace ExpectBetter
{
    internal static class Conditions
    {
        internal static T CheckNotNull<T>(T value, string name = "")
            where T : class
        {
            if (value == null)
                throw new ArgumentNullException(name);

            return value;
        }
    }
}
