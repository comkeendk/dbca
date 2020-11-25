using System;
using System.Collections.Generic;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Model.TaxRates
{
    public abstract class AbstractScheduledTaxRate : IScheduledTaxRate
    {
        public AbstractScheduledTaxRate(string municipalityName, decimal rate, Frequency frequency)
        {
            if(rate <= 0) { throw new ArgumentException("rate must be a positive decimal", nameof(rate)); }
            if(rate > 1) { throw new ArgumentException("rate must be less than 1", nameof(rate)); }

            MunicipalityName = municipalityName;
            Rate = rate;
            Frequency = frequency;
        }

        public string MunicipalityName { get; }

        public decimal Rate { get; }

        public Frequency Frequency { get; }

        public abstract IEnumerable<DateTime> Dates { get; }
    }
}