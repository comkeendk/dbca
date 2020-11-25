using System;

namespace Dbca.Municipalities.Abstractions.Services
{
    public interface IMunicipalityService
    {
        decimal GetScheduledTaxRate(string municipality, DateTime date);
        ImportResponse ImportScheduledTaxRates(string fullFilePath);
    }
}