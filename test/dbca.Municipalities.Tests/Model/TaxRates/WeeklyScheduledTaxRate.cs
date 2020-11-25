using System;
using System.Collections.Generic;
using System.Linq;
using Dbca.Municipalities.Model.TaxRates;
using Xunit;

namespace Dbca.Municipalities.Tests.Model.TaxRates
{
    public class WeeklyScheduledTaxRateTests
    {
        public class Constructor
        {
            [Theory]
            [MemberData(nameof(GetInvalidDateRanges))]
            public void GivenStartAndEndDate_WhenRangeIsInValid_ThrowsException(DateTime start, DateTime end)
            {
                Assert.Throws<NotSupportedException>(() => new WeeklyScheduledTaxRate(start, end, "municipality", 0.1m));
            }

            [Fact]
            public void GivenRate_WhenRateIsLessThanZero_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new WeeklyScheduledTaxRate(new DateTime(2020, 11, 23), new DateTime(2020, 11, 29), "municipality", -0.1m));
            }

            [Fact]
            public void GivenRate_WhenRateIsZero_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new WeeklyScheduledTaxRate(new DateTime(2020, 11, 23), new DateTime(2020, 11, 29), "municipality", 0m));
            }

            public static IEnumerable<object[]> GetInvalidDateRanges()
            {
                yield return new object[] { new DateTime(2020, 11, 23), new DateTime(2020, 11, 28) };
                yield return new object[] { new DateTime(2020, 11, 24), new DateTime(2020, 11, 29) };
                yield return new object[] { new DateTime(2020, 11, 23), new DateTime(2021, 11, 29) };
            }
        }

        public class Dates
        {
            [Fact]
            public void GeneratesExpectedDates()
            {
                var start = new DateTime(2020, 11, 23);
                var end = new DateTime(2020, 11, 29);
                
                var dates = new WeeklyScheduledTaxRate(start, end, "municipality", 0.1m).Dates.ToArray();

                var date = start.AddDays(0);
                int index = 0;
                while (date <= end)
                {
                    Assert.Equal(date, dates[index]);
                    date = date.AddDays(1);
                    index++;
                }
            }
        }
    }
}