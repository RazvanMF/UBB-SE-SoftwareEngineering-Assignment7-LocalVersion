using System.Text.RegularExpressions;
using NamespaceGPT.Common.Modules.BasicDataValidation.Module.Interfaces;

namespace NamespaceGPT.Common.BasicDataValidation.Module.Implementations.Validators
{
    public class PasswordValidator : IValidator
    {
        public bool Validate(string input)
        {
            if (input is not string password)
            {
                return false;
            }

            // Require at least 8 characters, including a number, a lowercase letter, an uppercase letter, and a special character
            return Regex.IsMatch(password, @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*\W)[A-Za-z\d\W]{8,}$");
        }
    }
}
