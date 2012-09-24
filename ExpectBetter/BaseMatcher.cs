using System;

using ExpectBetter.Matchers;

namespace ExpectBetter
{
    public class BaseMatcher<T, M>
        where M : BaseMatcher<T, M>
    {
        public M Not;
        protected bool inverted;
        protected T actual;
        protected string actualDescription;
        protected string expectedDescription;
        protected string failureMessage;
    }
}
