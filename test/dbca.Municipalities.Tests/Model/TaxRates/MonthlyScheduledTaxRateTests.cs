using System;
using System.Collections.Generic;
using System.Linq;
using Dbca.Municipalities.Model.TaxRates;
using Xunit;

namespace Dbca.Municipalities.Tests.Model.TaxRates
{
    public class MonthlyScheduledTaxRateTests
    {
        public class Constructor
        {
            [Theory]
            [MemberData(nameof(GetInvalidDateRanges))]
            public void GivenStartAndEndDate_WhenRangeIsInValid_ThrowsException(DateTime start, DateTime end)
            {
                Assert.Throws<NotSupportedException>(() => new MonthlyScheduledTaxRate(start, end, "municipality", 0.1m));
            }

            [Fact]
            public void GivenRate_WhenRateIsLessThanZero_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new MonthlyScheduledTaxRate(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31), "municipality", -0.1m));
            }

            [Fact]
            public void GivenRate_WhenRateIsZero_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new MonthlyScheduledTaxRate(new DateTime(2020, 1, 1), new DateTime(2020, 1, 31), "municipality", 0m));
            }

            public static IEnumerable<object[]> GetInvalidDateRanges()
            {
                yield return new object[] { new DateTime(2020, 2, 1), new DateTime(2020, 1, 28) };
                yield return new object[] { new DateTime(2020, 1, 1), new DateTime(2020, 2, 1) };
                yield return new object[] { new DateTime(2020, 1, 1), new DateTime(2021, 1, 31) };
            }
        }

        public class Dates
        {
            [Fact]
            public void GeneratesExpectedDates()
            {
                var start = new DateTime(2020, 1, 1);
                var end = new DateTime(2020, 1, 31);
                
                var dates = new MonthlyScheduledTaxRate(start, end, "municipality", 0.1m).Dates.ToArray();

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