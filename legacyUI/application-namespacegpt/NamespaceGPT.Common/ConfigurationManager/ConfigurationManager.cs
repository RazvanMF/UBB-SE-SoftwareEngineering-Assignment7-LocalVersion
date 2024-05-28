using NamespaceGPT.Common.Modules.ConfigurationParser.Module.JsonManager;
using NamespaceGPT.Common.Modules.ConfigurationParser.Module.JsonManager.JsonParser;

namespace NamespaceGPT.Common.ConfigurationManager
{
    public class ConfigurationManager : IConfigurationManager
    {
        public string GetConnectionString(string filePath)
        {
            var jsonObject = (JsonObject?)JsonFileReader.ParseJsonFile(filePath);

            var connectionString = (string?)((JsonObject?)jsonObject?.Properties["ConnectionStrings"])?.Properties["defaultConnection"];

            if (connectionString == null)
            {
                return string.Empty;
            }

            return connectionString;
        }
    }
}
