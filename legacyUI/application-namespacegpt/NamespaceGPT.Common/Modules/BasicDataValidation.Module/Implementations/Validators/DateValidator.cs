using System.Globalization;
using NamespaceGPT.Common.Modules.BasicDataValidation.Module.Interfaces;

namespace NamespaceGPT.Common.Modules.BasicDataValidation.Module.Implementations.Validators
{
    public class DateValidator : IValidator
    {
        public bool Validate(string input)
        {
            if (string.IsNullOrEmpty(input))
            {
                return false;
            }

            string[] formats = { "MM/dd/yyyy", "yyyy-MM-dd", "dd-MM-yyyy" };
            return DateTime.TryParseExact(input, formats, CultureInfo.InvariantCulture,
                DateTimeStyles.None, out _);
        }
    }
}
