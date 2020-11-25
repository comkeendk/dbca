using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Dbca.Municipalities.Abstractions.Model;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Abstractions.Strategies;

namespace Dbca.Municipalities.Model
{
    public class Municipality : IMunicipality
    {
        private readonly Dictionary<string, decimal> _taxRates;
        private readonly IScheduledTaxRateSortingStrategy _scheduledTaxRateSortingStrategy;
        public string Name { get; }

        public Municipality(
            IScheduledTaxRateSortingStrategy scheduledTaxRateSortingStrategy,
            string name,
            IEnumerable<IScheduledTaxRate> scheduledTaxRates)
        {
            if(string.IsNullOrWhiteSpace(name)) { throw new ArgumentException("A municipality must have a name", nameof(name)); }
            if(!(scheduledTaxRates?.Any() ?? false)) { throw new ArgumentException("At least one tax rate must be supplied", nameof(scheduledTaxRates)); }

            _scheduledTaxRateSortingStrategy = scheduledTaxRateSortingStrategy ?? throw new ArgumentNullException(nameof(scheduledTaxRateSortingStrategy));
            _taxRates = new Dictionary<string, decimal>();
            Name = name;

            GenerateScheduledTaxRatesPerDay(scheduledTaxRates);
        }

        public decimal GetTaxRate(DateTime date)
        {
            if(_taxRates.TryGetValue($"{date:ddMMyyyy}", out decimal scheduledRate))
            {
                return scheduledRate;
            }
            return 0m;
        }

        private void GenerateScheduledTaxRatesPerDay(IEnumerable<IScheduledTaxRate> scheduledTaxRates)
        {
            var sortedTaxRates = _scheduledTaxRateSortingStrategy.Sort(scheduledTaxRates);

            foreach (var scheduledTaxRate in sortedTaxRates)
            {
                foreach (var date in scheduledTaxRate.Dates)
                {
                    _taxRates[$"{date:ddMMyyyy}"] = scheduledTaxRate.Rate;
                }
            }
        }
    }
}