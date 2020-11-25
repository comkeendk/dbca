using System;
using System.Collections.Generic;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Model.TaxRates
{
    public class DailyScheduledTaxRate : AbstractScheduledTaxRate, IExactDateScheduledTaxRate
    {
        public DailyScheduledTaxRate(DateTime date, string municipalityName, decimal rate)
            : base(municipalityName, rate, Frequency.Daily)
        {
            Date = date;
        }

        public DateTime Date { get; }

        public override IEnumerable<DateTime> Dates 
        {
            get
            {
                yield return Date;
            }
        }
    }
}