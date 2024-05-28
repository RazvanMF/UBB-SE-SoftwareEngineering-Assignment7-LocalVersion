using System.Text;

namespace NamespaceGPT.Common.Modules.ConfigurationParser.Module.JsonManager
{
    public static class JsonFileReader
    {
        public static object? ParseJsonFile(string path)
        {
            try
            {
                string? line;
                StringBuilder jsonContent = new ();
                StreamReader jsonStreamReader = new (path);

                line = jsonStreamReader.ReadLine();

                while (line != null)
                {
                    jsonContent.Append(line.Trim());
                    line = jsonStreamReader.ReadLine();
                }

                return JsonParser.JsonParser.Parse(jsonContent.ToString());
            }
            catch (Exception)
            {
                // Handle exception...
            }

            return null;
        }
    }
}
