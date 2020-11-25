using System;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Abstractions.Factories
{
    public interface IScheduledTaxRateFactory
    {
        IScheduledTaxRate Create(string municipalityName, decimal rate, Frequency frequency, DateTime start, DateTime end);
    }
}