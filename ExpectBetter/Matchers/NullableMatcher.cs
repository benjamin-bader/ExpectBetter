namespace ExpectBetter.Matchers
{
    public class NullableMatcher<T> : BaseMatcher<T?, NullableMatcher<T>>
        where T : struct
    {
        [AllowNullActual]
        public virtual bool ToBeNull()
        {
            return !actual.HasValue;
        }

        [AllowNullActual]
        public virtual bool ToHaveAValue()
        {
            return actual.HasValue;
        }
    }
}
