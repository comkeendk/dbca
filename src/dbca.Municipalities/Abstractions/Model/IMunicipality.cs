using System;

namespace Dbca.Municipalities.Abstractions.Model
{
    public interface IMunicipality
    {
        string Name { get; }
        decimal GetTaxRate(DateTime date);
    }
}