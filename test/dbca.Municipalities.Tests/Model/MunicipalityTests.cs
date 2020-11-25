using System;
using System.Collections.Generic;
using System.Linq;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Abstractions.Strategies;
using Dbca.Municipalities.Model;
using Dbca.Municipalities.Model.TaxRates;
using Dbca.Municipalities.Strategies;
using Xunit;

namespace Dbca.Municipalities.Tests.Model
{
    public class MunicipalityTests
    {
        private const string municipalityName = "municipality";
        private const decimal yearlyRate = 0.2m;
        private const decimal monthRate = 0.4m;
        private const decimal dailyRate = 0.1m;

        public class Constructor : MunicipalityTests
        {
            [Fact]
            public void GivenSortingStrategy_WhenNull_ThrowsException()
            {
                Assert.Throws<ArgumentNullException>(() => new Municipality(null, municipalityName, CreateTaxRates()));
            }

            [Fact]
            public void GivenName_WhenNull_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new Municipality(CreateSortingStrategy(), null, CreateTaxRates()));
            }

            [Fact]
            public void GivenTaxRates_WhenNull_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new Municipality(CreateSortingStrategy(), municipalityName, null));
            }

            [Fact]
            public void GivenTaxRates_WhenEmpty_ThrowsException()
            {
                Assert.Throws<ArgumentException>(() => new Municipality(CreateSortingStrategy(), municipalityName, Enumerable.Empty<IScheduledTaxRate>()));
            }
        }

        public class GetTaxRate : MunicipalityTests
        {
            [Theory]
            [MemberData(nameof(GetExpectedRates))]
            public void ReturnsExpectedRate(DateTime date, decimal expectedRate)
            {
                var municipality = CreateMunicipality();

                var rate = municipality.GetTaxRate(date);

                Assert.Equal(expectedRate, rate);
            }

            public static IEnumerable<object[]> GetExpectedRates()
            {
                yield return new object[] { new DateTime(2020, 1, 1), dailyRate };
                yield return new object[] { new DateTime(2020, 5, 3), monthRate };
                yield return new object[] { new DateTime(2020, 7, 10), yearlyRate };
                yield return new object[] { new DateTime(2020, 3, 16), yearlyRate };
            }
        }

        private Municipality CreateMunicipality()
        {
            return new Municipality(CreateSortingStrategy(), "municipality", CreateTaxRates());
        }

        private IScheduledTaxRateSortingStrategy CreateSortingStrategy()
        {
            return new FrequencyScheduledTaxRateSortingStrategy();
        }

        private IEnumerable<IScheduledTaxRate> CreateTaxRates()
        {
            yield return new YearlyScheduledTaxRate(new DateTime(2020, 1, 1), new DateTime(2020, 12, 31), municipalityName, yearlyRate);
            yield return new MonthlyScheduledTaxRate(new DateTime(2020, 5, 1), new DateTime(2020, 5, 31), municipalityName, monthRate);
            yield return new DailyScheduledTaxRate(new DateTime(2020, 1, 1), municipalityName, dailyRate);
            yield return new DailyScheduledTaxRate(new DateTime(2020, 12, 25), municipalityName, dailyRate);
        }
    }
}