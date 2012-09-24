using System;

using ExpectBetter.Matchers;

namespace ExpectBetter.Matchers
{
    public class BoolMatcher : BaseMatcher<bool, BoolMatcher>
    {
        public virtual bool ToBeTrue()
        {
            return actual == true;
        }

        public virtual bool ToBeFalse()
        {
            return !actual;
        }

        public virtual bool ToEqual(bool expected)
        {
            return actual == expected;
        }
    }
}
