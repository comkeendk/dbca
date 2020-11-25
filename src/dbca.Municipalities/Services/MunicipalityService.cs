using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Dbca.Municipalities.Abstractions.Factories;
using Dbca.Municipalities.Abstractions.Model.TaxRates;
using Dbca.Municipalities.Abstractions.Repositories;
using Dbca.Municipalities.Abstractions.Services;

namespace Dbca.Municipalities.Services
{
    public class MunicipalityService : IMunicipalityService
    {
        private readonly IMunicipalityRepository _municipalityRepository;
        private readonly IScheduledTaxRateFactory _scheduledTaxRateFactory;

        public MunicipalityService(
            IMunicipalityRepository municipalityRepositiory,
            IScheduledTaxRateFactory scheduledTaxRateFactory
        )
        {
            _municipalityRepository = municipalityRepositiory ?? throw new ArgumentNullException(nameof(municipalityRepositiory));
            _scheduledTaxRateFactory = scheduledTaxRateFactory ?? throw new ArgumentNullException(nameof(scheduledTaxRateFactory));
        }
        public decimal GetScheduledTaxRate(string municipalityName, DateTime date)
        {
            var municipality = _municipalityRepository.GetMunicipality(municipalityName);

            return municipality?.GetTaxRate(date) ?? 0m;
        }

        public ImportResponse ImportScheduledTaxRates(string fullFilePath)
        {
            var response = new ImportResponse();

            if(string.IsNullOrWhiteSpace(fullFilePath))
            {
                response.AddError("No file specified");
                return response;
            }
            if(!fullFilePath.EndsWith(".csv"))
            {
                response.AddError("file must be in .csv format");
                return response;
            }
            if(!File.Exists(fullFilePath))
            {
                response.AddError("File does not exist");
                return response;
            }
            var lines = File.ReadAllLines(fullFilePath);
            for(int i = 1; i < lines.Length; i++)
            {
                var lineParts = lines[i].Split(';');
                if(lineParts.Length != 4)
                {
                    response.AddFailedLine(i, "Not able to find all elements");
                    continue;
                }
                var municipality = lineParts[0];
                var partsAreValid = true;
                if(string.IsNullOrWhiteSpace(municipality))
                {
                    response.AddFailedLine(i, "No municipality name specified");
                    partsAreValid = false;
                }
                if(!decimal.TryParse(lineParts[1], out decimal rate))
                {
                    response.AddFailedLine(i, $"Unable to parse rate {lineParts[1]} to a decimal");
                    partsAreValid = false;
                }
                var frequencyParses = Enum.TryParse(lineParts[2], true, out Frequency frequency) && Enum.IsDefined(typeof(Frequency), frequency);
                if(!frequencyParses)
                {
                    response.AddFailedLine(i, $"Unable to parse {lineParts[2]} to a valid frequency");
                    partsAreValid = false;
                }

                if(frequencyParses)
                {
                    var (Success, ErrorMessage, Ranges) = ParseDateRanges(lineParts[3], frequency);
                    if(!Success)
                    {
                        partsAreValid = false;
                        response.AddFailedLine(i, ErrorMessage);
                    }

                    if(!partsAreValid) { continue; }

                    foreach(var (Start, End) in Ranges)
                    {
                        var scheduledTaxRate = _scheduledTaxRateFactory.Create(municipality, rate, frequency, Start, End);
                        _municipalityRepository.SaveScheduledTaxRate(scheduledTaxRate);
                    }
                }
            }

            return response;
        }

        private (bool Success, string ErrorMessage, List<(DateTime Start, DateTime End)> Ranges) ParseDateRanges(string dateRangesString, Frequency frequency)
        {
            if(string.IsNullOrWhiteSpace(dateRangesString)) { return (false, "No dates supplied", null); }
            
            List<(DateTime Start, DateTime End)> ranges = new List<(DateTime Start, DateTime End)>();
            StringBuilder errorsSb = new StringBuilder();
            var stringRanges = dateRangesString.Split(',');
            var dailyDatePattern = @"\d{2}.\d{2}.\d{4}";
            var periodicalDatePattern = $@"{dailyDatePattern}-{dailyDatePattern}";

            foreach(var dateRangeString in stringRanges)
            {
                if(frequency == Frequency.Daily)
                {
                    if(!Regex.IsMatch(dateRangeString, dailyDatePattern))
                    {
                        errorsSb.AppendLine("Unsupported date format for daily frequency tax rates");
                        continue;
                    }
                    var date = ParseDateFromString(dateRangeString);
                    ranges.Add((date, date));
                }
                else
                {
                    if(!Regex.IsMatch(dateRangeString, periodicalDatePattern))
                    {
                        errorsSb.AppendLine("Unsupported date format for periodically scheduled tax rates");
                        continue;
                    }
                    var startAndEnd = dateRangeString.Split('-');
                    ranges.Add((ParseDateFromString(startAndEnd[0]), ParseDateFromString(startAndEnd[1])));
                }
            }

            return (errorsSb.Length == 0, errorsSb.ToString(), ranges);
        }

        private DateTime ParseDateFromString(string dateString)
        {
            var parts = dateString.Split('.');
            var numericParts = parts.Select(p => Convert.ToInt32(p)).ToArray();

            return new DateTime(numericParts[2], numericParts[1], numericParts[0]);
        }
    }
}