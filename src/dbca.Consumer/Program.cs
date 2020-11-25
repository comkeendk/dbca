using System;
using Dbca.Municipalities.Abstractions.Factories;
using Dbca.Municipalities.Abstractions.Repositories;
using Dbca.Municipalities.Abstractions.Services;
using Dbca.Municipalities.Factories;
using Dbca.Municipalities.Repositories;
using Dbca.Municipalities.Services;

namespace dbca.Consumer
{
    class Program
    {
        static void Main()
        {
            var municipalityService = CreateMunicipalityService();

            Console.WriteLine("Fetching tax rate for Copenhagen January 1st 2020");
            Console.WriteLine($"Result {municipalityService.GetScheduledTaxRate("Copenhagen", new DateTime(2020, 1, 1))}");

            Console.WriteLine("Fetching tax rate for Århus January 1st 2020");
            Console.WriteLine($"Result {municipalityService.GetScheduledTaxRate("Århus", new DateTime(2020, 1, 1))}");

            Console.ReadLine();
        }

        private static IMunicipalityService CreateMunicipalityService()
        {
            return new MunicipalityService(CreateMunicipalityRepository(), CreateScheduledTaxRateFactory());
        }

        private static IMunicipalityRepository CreateMunicipalityRepository()
        {
            return new InMemoryMunicipalityRepository(CreateScheduledTaxRateFactory());
        }

        private static IScheduledTaxRateFactory CreateScheduledTaxRateFactory()
        {
            return new ScheduledTaxRateFactory();
        }
    }
}
