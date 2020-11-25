using System;

namespace Dbca.Municipalities.Abstractions.Model.TaxRates
{
    public interface IExactDateScheduledTaxRate : IScheduledTaxRate
    {
        DateTime Date { get; }
    }
}