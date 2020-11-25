using System;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Model.TaxRates
{
    public class YearlyScheduledTaxRate : PeriodicallyScheduledTaxRate
    {
        public YearlyScheduledTaxRate(
            DateTime start,
            DateTime end,
            string municipalityName,
            decimal rate)
            : base(start, end, municipalityName, rate, Frequency.Yearly)
        { }

        protected override bool ValidateRange(DateTime start, DateTime end)
        {
            return start.Day == 1 &&
                start.Month == 1 &&
                end.Day == 31 &&
                end.Month == 12 &&
                start.Year == end.Year;
        }
    }
}