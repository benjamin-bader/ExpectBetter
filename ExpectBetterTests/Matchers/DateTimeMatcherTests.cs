using System;

using NUnit.Framework;

using ExpectBetter;

namespace ExpectBetterTests.Matchers
{
    [TestFixture]
    public class DateTimeMatcherTests
    {
        private DateTime universalTime;
        private DateTime localTime;
        private DateTime unspecifiedTime;

        [SetUp]
        public void Setup()
        {
            universalTime = DateTime.UtcNow;
            localTime = DateTime.Now;
            unspecifiedTime = DateTime.Parse("2 October 2012, 12:00");
        }

        [Test]
        public void ToBeLaterThan_WhenActualIsLaterThanExpected_ReturnsTrue()
        {
            var actual = DateTime.UtcNow.AddMinutes(1);
            Expect.The(actual).ToBeLaterThan(DateTime.UtcNow);
        }

        [Test]
        public void ToBeLaterThan_ConvertsLocalDatesToUtc()
        {
            Expect.The(localTime).Not.ToBeLaterThan(universalTime);
        }

        [Test, Throws]
        public void ToBeLaterThan_WhenActualEqualsExpected_Throws()
        {
            Expect.The(DateTime.Now).ToBeLaterThan(DateTime.Now);
        }

        [Test, Throws]
        public void ToBeLaterThan_WhenActualIsEarlierThanExpected_Throws()
        {
            var actual = DateTime.Now.AddSeconds(-1);
            Expect.The(actual).ToBeLaterThan(DateTime.Now);
        }

        [Test]
        public void ToHaveKind_WhenActualIsLocalAsExpected_ReturnsTrue()
        {
            Expect.The(localTime).ToHaveKind(DateTimeKind.Local);
        }

        [Test]
        public void ToHaveKind_WhenActualIsUtcAsExpected_ReturnsTrue()
        {
            Expect.The(universalTime).ToHaveKind(DateTimeKind.Utc);
        }

        [Test, Throws]
        public void ToHaveKind_WhenLocalExpectedButActualIsUnspecified_Throws()
        {
            Expect.The(unspecifiedTime).ToHaveKind(DateTimeKind.Local);
        }

        [Test]
        public void ToBeUtc_WhenActualIsUtc_ReturnsTrue()
        {
            Expect.The(universalTime).ToBeUtc();
        }

        [Test, Throws]
        public void ToBeUtc_WhenActualIsLocal_Throws()
        {
            Expect.The(localTime).ToBeUtc();
        }

        [Test, Throws]
        public void ToBeUtc_WhenActualIsUnspecified_Throws()
        {
            Expect.The(unspecifiedTime).ToBeUtc();
        }

        [Test]
        public void ToBeInTheMonthOf_WhenMonthIsAsExpected_ReturnsTrue()
        {
            var january   = new DateTime(2012, 1, 1);
            var february  = new DateTime(2012, 2, 1);
            var march     = new DateTime(2012, 3, 1);
            var april     = new DateTime(2012, 4, 1);
            var may       = new DateTime(2012, 5, 1);
            var june      = new DateTime(2012, 6, 1);
            var july      = new DateTime(2012, 7, 1);
            var august    = new DateTime(2012, 8, 1);
            var september = new DateTime(2012, 9, 1);
            var october   = new DateTime(2012, 10, 1);
            var november  = new DateTime(2012, 11, 1);
            var december  = new DateTime(2012, 12, 1);

            Expect.The(january).ToBeInTheMonthOf(1);
            Expect.The(february).ToBeInTheMonthOf(2);
            Expect.The(march).ToBeInTheMonthOf(3);
            Expect.The(april).ToBeInTheMonthOf(4);
            Expect.The(may).ToBeInTheMonthOf(5);
            Expect.The(june).ToBeInTheMonthOf(6);
            Expect.The(july).ToBeInTheMonthOf(7);
            Expect.The(august).ToBeInTheMonthOf(8);
            Expect.The(september).ToBeInTheMonthOf(9);
            Expect.The(october).ToBeInTheMonthOf(10);
            Expect.The(november).ToBeInTheMonthOf(11);
            Expect.The(december).ToBeInTheMonthOf(12);
        }

        [Test, Throws]
        public void ToBeInTheMonthOf_WhenMonthIsUnexpected_Throws()
        {
            var januaryDate = new DateTime(2012, 1, 1);
            Expect.The(januaryDate).ToBeInTheMonthOf(2);
        }
    }
}
