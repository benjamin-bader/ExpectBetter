using System;
using System.Collections;
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
            actualDesc = "[" + (actualDesc ?? ToStringRespectingNullsAndEnumerables(actual)) + "]";
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
                .Stringify(ToStringRespectingNullsAndEnumerables)
                .ToString();
        }

        private static string ToStringRespectingNullsAndEnumerables(object obj)
        {
            if (ReferenceEquals(obj, null))
            {
                return "null";
            }

            if (obj is IEnumerable)
            {
                var sb = new StringBuilder("{");

                return (obj as IEnumerable)
                    .Cast<object>()
                    .Stringify(o => o.ToString(), sb)
                    .Append("}")
                    .ToString();
            }
            
            return obj.ToString();
        }

        private static StringBuilder Stringify(
            this IEnumerable<object> collection,
            Func<object, string> toString,
            StringBuilder sb = null)
        {
            if (sb == null)
            {
                sb = new StringBuilder();
            }

            return collection
                .Select(toString)
                .Select(str => "[" + str + "]")
                .Interject(new[] { ", " })
                .Aggregate(sb, (sbb, str) => sbb.Append(str));
        }
    }
}
