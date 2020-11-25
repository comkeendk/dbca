using System.Collections.Generic;
using System.Linq;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Abstractions.Strategies;

namespace Dbca.Municipalities.Strategies
{
    public class FrequencyScheduledTaxRateSortingStrategy : IScheduledTaxRateSortingStrategy
    {
        public IEnumerable<IScheduledTaxRate> Sort(IEnumerable<IScheduledTaxRate> scheduledTaxRates)
        {
            return scheduledTaxRates.OrderBy(t => t.Frequency);
        }
    }
}