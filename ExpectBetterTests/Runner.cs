using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using ExpectBetter;

namespace ExpectBetterTests
{
    public class Runner
    {
        public static void Main(string[] args)
        {
            var tests = new EnumerableMatcherTests();
            try
            {
                tests.ToContain_UsesEqualityMatcher();
            }
            catch (ExpectationException)
            { }

            try
            {
                tests.ToContainInOrder_WhenTrue_ReturnsTrue();
            }
            catch (ExpectationException)
            { }

            try
            {
                tests.ToContain_WhenExpectedNotInEnumerable_Throws();
            }
            catch (ExpectationException)
            {

            }
        }
    }
}
