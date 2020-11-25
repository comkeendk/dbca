using System.Linq;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Strategies;
using Moq;
using Xunit;

namespace Dbca.Municipalities.Tests.Strategies
{
    public class FrequencyScheduledTaxRateSortingStrategyTests 
    {
        private readonly FrequencyScheduledTaxRateSortingStrategy _strategy;

        public FrequencyScheduledTaxRateSortingStrategyTests()
        {
            _strategy = new FrequencyScheduledTaxRateSortingStrategy();
        }

        public class Sort : FrequencyScheduledTaxRateSortingStrategyTests
        {
            private readonly Mock<IScheduledTaxRate> _yearlyTaxRateMock;
            private readonly Mock<IScheduledTaxRate> _monthlyTaxRateMock;
            private readonly Mock<IScheduledTaxRate> _dailyTaxRateMock;

            public Sort()
                : base()
            {
                _yearlyTaxRateMock = new Mock<IScheduledTaxRate>();
                _yearlyTaxRateMock.Setup(m => m.Frequency).Returns(Frequency.Yearly);
                _monthlyTaxRateMock = new Mock<IScheduledTaxRate>();
                _monthlyTaxRateMock.Setup(m => m.Frequency).Returns(Frequency.Monthly);
                _dailyTaxRateMock = new Mock<IScheduledTaxRate>();
                _dailyTaxRateMock.Setup(m => m.Frequency).Returns(Frequency.Daily);
            }

            [Fact]
            public void SortsCorrectly()
            {
                var expectedOrder = new [] { Frequency.Yearly, Frequency.Monthly, Frequency.Daily };
                var unsortedTaxRates = new [] { _monthlyTaxRateMock.Object, _dailyTaxRateMock.Object, _yearlyTaxRateMock.Object };

                var sortedTaxRates = _strategy.Sort(unsortedTaxRates).ToList();

                for (int i = 0; i < sortedTaxRates.Count; i++)
                {
                    Assert.Equal(expectedOrder[i], sortedTaxRates[i].Frequency);
                }
            }
        }
    }
}