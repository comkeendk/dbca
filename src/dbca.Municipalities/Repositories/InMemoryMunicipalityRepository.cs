using System;
using System.Collections.Generic;
using System.Linq;
using Dbca.Municipalities.Abstractions.Factories;
using Dbca.Municipalities.Abstractions.Model;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Abstractions.Repositories;
using Dbca.Municipalities.Model;
using Dbca.Municipalities.Strategies;

namespace Dbca.Municipalities.Repositories
{
    public class InMemoryMunicipalityRepository : IMunicipalityRepository
    {
        private readonly IScheduledTaxRateFactory _scheduledTaxRateFactory;
        private readonly IList<(string MunicipalityName, decimal Rate, Frequency Frequency, DateTime Start, DateTime End)> _rows;

        public InMemoryMunicipalityRepository(IScheduledTaxRateFactory scheduledTaxRateFactory)
        {
            _scheduledTaxRateFactory = scheduledTaxRateFactory ?? throw new ArgumentNullException(nameof(scheduledTaxRateFactory));
            _rows = new List<(string MunicipalityName, decimal Rate, Frequency Frequency, DateTime Start, DateTime End)>();

            SeedRows();
        }
        
        public IMunicipality GetMunicipality(string municipalityName)
        {
            if(string.IsNullOrWhiteSpace(municipalityName)) { throw new ArgumentException("Municipality name must be provided", nameof(municipalityName)); }

            var rowsForMunicipality = _rows.Where(r => r.MunicipalityName.Equals(municipalityName, StringComparison.InvariantCultureIgnoreCase));

            if(rowsForMunicipality.Any())
            {
                return new Municipality(
                    new FrequencyScheduledTaxRateSortingStrategy(),
                    municipalityName,
                    rowsForMunicipality.Select(r => _scheduledTaxRateFactory.Create(r.MunicipalityName, r.Rate, r.Frequency, r.Start, r.End))
                );
            }

            return null;
        }

        public bool SaveScheduledTaxRate(IScheduledTaxRate scheduledTaxRate)
        {
            throw new System.NotImplementedException();
        }

        private void SeedRows()
        {
            _rows.Add(("Copenhagen", 0.2m, Frequency.Yearly, new DateTime(2020, 1, 1), new DateTime(2020, 12, 31)));
            _rows.Add(("Copenhagen", 0.4m, Frequency.Monthly, new DateTime(2020, 5, 1), new DateTime(2020, 5, 31)));
            _rows.Add(("Copenhagen", 0.1m, Frequency.Daily, new DateTime(2020, 1, 1), new DateTime(2020, 1, 1)));
            _rows.Add(("Copenhagen", 0.1m, Frequency.Daily, new DateTime(2020, 12, 25), new DateTime(2020, 12, 25)));
        }
    }
}