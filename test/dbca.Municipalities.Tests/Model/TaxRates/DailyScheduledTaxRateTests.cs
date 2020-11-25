using System;
using Dbca.Municipalities.Model.TaxRates;
using Xunit;

namespace Dbca.Municipalities.Tests.Model.TaxRates
{
    public class DailyScheduledRateTests
    {
        public class Constructor
        {
            [Fact]
            public void GivenRate_WhenRateIsLessThanZero_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new DailyScheduledTaxRate(new DateTime(2020, 1, 1), "municipality", -0.1m));
            }

            [Fact]
            public void GivenRate_WhenRateIsZero_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new DailyScheduledTaxRate(new DateTime(2020, 1, 1), "municipality", 0m));
            }
        }
    }
}