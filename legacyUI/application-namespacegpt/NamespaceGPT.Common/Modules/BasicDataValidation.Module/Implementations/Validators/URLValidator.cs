using NamespaceGPT.Common.Modules.BasicDataValidation.Module.Interfaces;

namespace NamespaceGPT.Common.Modules.BasicDataValidation.Module.Implementations.Validators
{
    public class URLValidator : IValidator
    {
        public bool Validate(string input)
        {
            if (input is not string url)
            {
                return false;
            }

            return Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                   && (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
