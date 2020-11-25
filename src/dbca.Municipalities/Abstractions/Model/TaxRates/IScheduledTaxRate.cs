using System;
using System.Collections.Generic;

namespace Dbca.Municipalities.Abstractions.Model.TaxRates
{
    public interface IScheduledTaxRate
    {
        string MunicipalityName { get; }
        decimal Rate { get; }
        Frequency Frequency { get; }

        IEnumerable<DateTime> Dates { get; }
    }
}