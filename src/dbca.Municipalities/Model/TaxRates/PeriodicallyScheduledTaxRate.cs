using System;
using System.Collections.Generic;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Model.TaxRates
{
    public abstract class PeriodicallyScheduledTaxRate : AbstractScheduledTaxRate, IPeriodicallyScheduledTaxRate
    {
        public PeriodicallyScheduledTaxRate(DateTime start, DateTime end, string municipalityName, decimal rate, Frequency frequency)
            : base(municipalityName, rate, frequency)
        {
            if(!ValidateRange(start, end)) { throw new NotSupportedException("Supplied date range is not valid. Startdate must be January 1st and end date must be December 31t same year"); }

            Start = start;
            End = end;
        }

        public DateTime Start { get; }

        public DateTime End { get; }

        public override IEnumerable<DateTime> Dates
        {
            get
            {
                var date = Start.AddDays(0);
                while (date <= End)
                {
                    yield return date;
                    date = date.AddDays(1);
                }
            }
        }

        protected abstract bool ValidateRange(DateTime start, DateTime end);
    }
}