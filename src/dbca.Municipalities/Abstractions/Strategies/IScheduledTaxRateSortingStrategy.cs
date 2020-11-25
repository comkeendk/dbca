using System.Collections.Generic;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Abstractions.Strategies
{
    public interface IScheduledTaxRateSortingStrategy
    {
        IEnumerable<IScheduledTaxRate> Sort(IEnumerable<IScheduledTaxRate> scheduledTaxRates);
    }
}