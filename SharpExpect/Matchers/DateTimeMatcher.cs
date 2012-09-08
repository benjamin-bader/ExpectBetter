using System;

namespace SharpExpect
{
	public class DateTimeMatcher : BaseMatcher<DateTime, DateTimeMatcher>
	{
		public virtual bool ToBeEarlierThan(DateTime expected)
		{
			return actual.CompareTo(expected) < 0;
		}

		public virtual bool ToBeLaterThan(DateTime expected)
		{
			return actual.CompareTo(expected) > 0;
		}

		public virtual bool ToHaveKind(DateTimeKind kind)
		{
			return actual.Kind == kind;
		}

		public virtual bool ToBeDaylightSavingsTime()
		{
			return actual.IsDaylightSavingTime();
		}

		public virtual bool ToBeUtc()
		{
			return actual.Kind == DateTimeKind.Utc;
		}

		public virtual bool ToBeOnA(DayOfWeek dayOfWeek)
		{
			return actual.DayOfWeek == dayOfWeek;
		}

		public virtual bool ToBeInTheMonthOf(int expectedMonth)
		{
			return actual.Month == expectedMonth;
		}

		public virtual bool ToEqual(DateTime expected)
		{
			return actual.CompareTo(expected) == 0;
		}
	}
}

