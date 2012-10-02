using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ExpectBetter.Codegen
{
    internal static class Errors
    {
        private const string BugReportMessage = "If you see this message, you have encounted a bug in ExpectBetter.  Please report this error (and stack trace) to the developers at https://github.com/benjamin-bader/ExpectBetter.  Thanks, and we're sorry!";

        public static void IllegalNullActual()
        {
            throw new ExpectationException("Actual cannot be null.");
        }

        public static void GiveBugReport(Exception ex)
        {
            throw new ExpectationException(BugReportMessage, ex);
        }

        public static void BadMatch(string actualDesc, string expectedDesc, bool inverted, object actual, string methodName, object[] expectedArgs)
        {
            actualDesc = "[" + (actualDesc ?? ToStringRespectingNulls(actual)) + "]";
            expectedDesc = expectedDesc ?? DescriptionOfExpected(expectedArgs);
            var message = new StringBuilder("Failure: ")
                .Append("Expected ")
                .Append(actualDesc)
                .Append(inverted ? " not" : "")
                .Append(System.Text.RegularExpressions.Regex.Replace(methodName, "([A-Z])", " $1").ToLowerInvariant())
                .Append(" ")
                .Append(expectedDesc)
                .ToString();

            Console.WriteLine(message);

            throw new ExpectationException(message);
        }

        private static string DescriptionOfExpected(object[] expectedArgs)
        {
            return expectedArgs
                .Select(ToStringRespectingNulls)
                .Select(str => "[" + str + "]")
                .Interject(new[] { ", " })
                .Aggregate(new StringBuilder(), (sb, str) => sb.Append(str))
                .ToString();
        }

        private static string ToStringRespectingNulls(object obj)
        {
            return ReferenceEquals(obj, null)
                ? "null"
                : obj.ToString();
        }
    }
}
