namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes assertions on nullable types.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class NullableMatcher<T> : BaseMatcher<T?, NullableMatcher<T>>
        where T : struct
    {
        /// <summary>
        /// Expect the value to be <see langword="null"/>.
        /// </summary>
        [AllowNullActual]
        public virtual bool ToBeNull()
        {
            return !actual.HasValue;
        }

        /// <summary>
        /// Expect the value not to be <see langword="null"/>.
        /// </summary>
        [AllowNullActual]
        public virtual bool ToHaveAValue()
        {
            return actual.HasValue;
        }
    }
}
