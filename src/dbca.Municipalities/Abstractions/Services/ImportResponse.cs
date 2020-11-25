using System.Collections.Generic;

namespace Dbca.Municipalities.Abstractions.Services
{
    public class ImportResponse
    {
        private readonly IList<string> _errors;
        private readonly IDictionary<int, string> _linesWithErrors;
        public bool Success => _errors.Count == 0 && _linesWithErrors.Count == 0;

        public ImportResponse()
        {
            _errors = new List<string>();
            _linesWithErrors = new Dictionary<int, string>();
        }

        public void AddError(string error)
        {
            _errors.Add(error);
        }

        public void AddFailedLine(int lineNumber, string errorMessage)
        {
            if(_linesWithErrors.TryGetValue(lineNumber, out string existingValue))
            {
                _linesWithErrors[lineNumber] = $"{existingValue}\n{errorMessage}";
                return;
            }
            _linesWithErrors[lineNumber] = errorMessage;
        }
    }
}