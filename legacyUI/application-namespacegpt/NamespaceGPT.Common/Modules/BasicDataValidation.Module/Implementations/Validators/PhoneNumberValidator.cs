using System.Text.RegularExpressions;
using NamespaceGPT.Common.Modules.BasicDataValidation.Module.Interfaces;

namespace NamespaceGPT.Common.BasicDataValidation.Module.Implementations.Validators
{
    public class PhoneNumberValidator : IValidator
    {
        public bool Validate(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return false;
            }

            string pattern = @"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$";
            return Regex.IsMatch(input, pattern);
        }
    }
}
