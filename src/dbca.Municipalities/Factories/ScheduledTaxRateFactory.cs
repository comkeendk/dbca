using System;
using Dbca.Municipalities.Abstractions.Factories;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Model.TaxRates;

namespace Dbca.Municipalities.Factories
{
    public class ScheduledTaxRateFactory : IScheduledTaxRateFactory
    {
        public IScheduledTaxRate Create(string municipalityName, decimal rate, Frequency frequency, DateTime start, DateTime end)
        {
            switch (frequency)
            {
                case Frequency.Yearly:
                    return new YearlyScheduledTaxRate(start, end, municipalityName, rate);
                case Frequency.Monthly:
                    return new MonthlyScheduledTaxRate(start, end, municipalityName, rate);
                case Frequency.Weekly:
                    return new WeeklyScheduledTaxRate(start, end, municipalityName, rate);
                case Frequency.Daily:
                    return new DailyScheduledTaxRate(start, municipalityName, rate);
                default:
                    throw new NotSupportedException($"Unsupported frequency {frequency}");
            }
        }
    }
}