using System.Text.RegularExpressions;
using NamespaceGPT.Common.Modules.BasicDataValidation.Module.Interfaces;

namespace NamespaceGPT.Common.Modules.BasicDataValidation.Module.Implementations.Validators
{
    public class UsernameValidator : IValidator
    {
        public bool Validate(string input)
        {
            if (input is not string username)
            {
                return false;
            }

            // Allow only alphanumeric characters and underscores, length between 3 and 24 characters
            return Regex.IsMatch(username, @"^[a-zA-Z0-9_]{3,24}$");
        }
    }
}
