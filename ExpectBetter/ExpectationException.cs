using System;

namespace ExpectBetter
{
    /// <summary>
    /// Represents an unmet expectation.
    /// </summary>
    [Serializable]
    public class ExpectationException : ApplicationException
    {
        /// <summary>
        /// Creates an instance of the <see cref="ExpectationException"/> class
        /// with the given <paramref name="message"/>.
        /// </summary>
        public ExpectationException(string message)
            : this(message, null)
        {
        }

        /// <summary>
        /// Creates an instance of the <see cref="ExpectationException"/> class
        /// with the given <paramref name="message"/> and
        /// <paramref name="innerException"/> object.
        /// </summary>
        /// <param name="message"></param>
        /// <param name="innerException"></param>
        public ExpectationException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
