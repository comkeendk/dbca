using System;

namespace Dbca.Municipalities.Abstractions.Model.TaxRates
{
    public interface IPeriodicallyScheduledTaxRate : IScheduledTaxRate
    {
        DateTime Start { get; }
        DateTime End { get; }
    }
}