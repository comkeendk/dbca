using System;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Model.TaxRates
{
    public class WeeklyScheduledTaxRate : PeriodicallyScheduledTaxRate
    {
        public WeeklyScheduledTaxRate(DateTime start, DateTime end, string municipalityName, decimal rate)
            : base(start, end, municipalityName, rate, Frequency.Weekly)
        { }

        protected override bool ValidateRange(DateTime start, DateTime end)
        {
            return start.DayOfWeek == DayOfWeek.Monday &&
                end == start.AddDays(6);
        }
    }
}