using System;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Model.TaxRates
{
    public class MonthlyScheduledTaxRate : PeriodicallyScheduledTaxRate
    {
        public MonthlyScheduledTaxRate(DateTime start, DateTime end, string municipalityName, decimal rate) 
            : base(start, end, municipalityName, rate, Frequency.Monthly)
        { }

        protected override bool ValidateRange(DateTime start, DateTime end)
        {
            return start.Day == 1 &&
                start.Month == end.Month &&
                start.Year == end.Year &&
                end.Day == start.AddMonths(1).AddDays(-1).Day;
        }
    }
}