using System;
using System.Globalization;

namespace ExpectBetter.Matchers
{
    /// <summary>
    /// Exposes test methods on <see cref="DateTime"/> values.
    /// </summary>
    public class DateTimeMatcher : BaseMatcher<DateTime, DateTimeMatcher>
    {
        /// <summary>
        /// Expect the date to fall before the given date.
        /// </summary>
        public virtual bool ToBeEarlierThan(DateTime expected)
        {
            return actual.CompareTo(expected) < 0;
        }

        /// <summary>
        /// Expect the date to come after the given date.
        /// </summary>
        public virtual bool ToBeLaterThan(DateTime expected)
        {
            return actual.CompareTo(expected) > 0;
        }

        /// <summary>
        /// Expect the date to be of a given <see cref="DateTimeKind"/>.
        /// </summary>
        public virtual bool ToHaveKind(DateTimeKind kind)
        {
            actualDescription = string.Format("{0} (kind = {1})", actual, actual.Kind);
            return actual.Kind == kind;
        }

        /// <summary>
        /// Expect the date to fall within daylight savings time.
        /// </summary>
        public virtual bool ToBeDaylightSavingsTime()
        {
            return actual.IsDaylightSavingTime();
        }

        /// <summary>
        /// Expect the date to be in UTC.
        /// </summary>
        public virtual bool ToBeUtc()
        {
            return actual.Kind == DateTimeKind.Utc;
        }

        /// <summary>
        /// Expect the date to fall on a given day of the week.
        /// </summary>
        public virtual bool ToBeOnA(DayOfWeek dayOfWeek)
        {
            actualDescription = "A " + actual.DayOfWeek;
            return actual.DayOfWeek == dayOfWeek;
        }

        /// <summary>
        /// Expect the date to fall within a given month.
        /// </summary>
        public virtual bool ToBeInTheMonthOf(int expectedMonth)
        {
            expectedDescription = CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(expectedMonth);
            return actual.Month == expectedMonth;
        }

        /// <summary>
        /// Expect the date to exactly equal a given date.
        /// </summary>
        public virtual bool ToEqual(DateTime expected)
        {
            return actual.CompareTo(expected) == 0;
        }
    }
}
