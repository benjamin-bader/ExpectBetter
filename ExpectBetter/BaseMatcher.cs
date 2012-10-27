namespace ExpectBetter
{
    /// <summary>
    /// Represents the attributes common to all matchers.
    /// </summary>
    /// <typeparam name="TActual">The type of the value being tested.</typeparam>
    /// <typeparam name="TMatcher">The actual type of the derived matcher.</typeparam>
    public class BaseMatcher<TActual, TMatcher>
        where TMatcher : BaseMatcher<TActual, TMatcher>
    {
        /// <summary>
        /// A matcher that negates all defined methods.
        /// </summary>
        public TMatcher Not;

        /// <summary>
        /// A value indicating whether this matcher should negate the results
        /// of its test methods; in other words, whether it is the
        /// <see cref="Not"/> value of another matcher.
        /// </summary>
        protected bool inverted;

        /// <summary>
        /// The value being tested.
        /// </summary>
        protected TActual actual;

        /// <summary>
        /// When set, specifies a message describing the value being tested.
        /// </summary>
        protected string actualDescription;

        /// <summary>
        /// When set, specifies a message describing the expected value.
        /// </summary>
        protected string expectedDescription;

        /// <summary>
        /// When set, specifies a test's failure message.
        /// </summary>
        protected string failureMessage;
    }
}
