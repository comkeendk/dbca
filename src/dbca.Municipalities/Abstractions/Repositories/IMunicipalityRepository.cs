using Dbca.Municipalities.Abstractions.Model;
using Dbca.Municipalities.Abstractions.Model.TaxRates;

namespace Dbca.Municipalities.Abstractions.Repositories
{
    public interface IMunicipalityRepository
    {
        IMunicipality GetMunicipality(string municipalityName);
        bool SaveScheduledTaxRate(IScheduledTaxRate scheduledTaxRate);
    }
}