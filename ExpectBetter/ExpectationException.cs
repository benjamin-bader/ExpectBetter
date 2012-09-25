using System;

namespace ExpectBetter
{
    /// <summary>
    /// Represents an unmet expectation.
    /// </summary>
    [Serializable]
    public class ExpectationException : ApplicationException
    {
        public ExpectationException(string message)
            : this(message, null)
        {
        }

        public ExpectationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
