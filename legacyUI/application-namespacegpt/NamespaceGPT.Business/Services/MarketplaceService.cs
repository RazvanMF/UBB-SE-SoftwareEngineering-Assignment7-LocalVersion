using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class MarketplaceService : IMarketplaceService
    {
        private readonly IMarketplaceRepository marketplaceRepository;

        public MarketplaceService(IMarketplaceRepository marketplaceRepository)
        {
            this.marketplaceRepository = marketplaceRepository ?? throw new ArgumentNullException(nameof(marketplaceRepository));
        }

        public int AddMarketplace(Marketplace marketplace)
        {
            try
            {
                HttpClient client = new HttpClient();
                string jsonHtmlBody = JsonConvert.SerializeObject(marketplace);
                jsonHtmlBody = jsonHtmlBody.Replace("name", "marketplaceName");
                jsonHtmlBody = jsonHtmlBody.Replace("countryoforigin", "country");
                StringContent content = new StringContent(jsonHtmlBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/marketplaces", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                Marketplace? result = JsonConvert.DeserializeObject<Marketplace>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                return result.Id;
            }
            catch
            {
                return -1;
            }
        }

        public int MarketplaceExists(Marketplace marketplace)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/marketplaces")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                responseBody = responseBody.Replace("marketplaceName", "name");
                responseBody = responseBody.Replace("country", "countryoforigin");
                List<Marketplace>? result = JsonConvert.DeserializeObject<List<Marketplace>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                result = result.ToList();

                List<Marketplace> parsedResult = new List<Marketplace>();

                foreach (Marketplace market in result)
                {
                    if (market != null
                        && market.Name == marketplace.Name
                        && market.WebsiteURL == marketplace.WebsiteURL
                        && market.CountryOfOrigin == marketplace.CountryOfOrigin)
                    {
                        parsedResult.Add(market);
                    }
                }

                if (parsedResult.Count > 0)
                {
                    return parsedResult[0].Id;
                }
                return -1;
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteMarketplace(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync("https://localhost:7040/api/marketplaces/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<Marketplace> GetAllMarketplaces()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/marketplaces")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                responseBody = responseBody.Replace("marketplaceName", "name");
                responseBody = responseBody.Replace("country", "countryoforigin");
                List<Marketplace>? result = JsonConvert.DeserializeObject<List<Marketplace>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public Marketplace? GetMarketplace(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/marketplaces" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                responseBody = responseBody.Replace("marketplaceName", "name");
                responseBody = responseBody.Replace("country", "countryoforigin");
                Marketplace? result = JsonConvert.DeserializeObject<Marketplace>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateMarketplace(int id, Marketplace marketplace)
        {
            try
            {
                HttpClient client = new HttpClient();
                string jsonHtmlBody = JsonConvert.SerializeObject(marketplace);
                jsonHtmlBody = jsonHtmlBody.Replace("name", "marketplaceName");
                jsonHtmlBody = jsonHtmlBody.Replace("countryoforigin", "country");
                StringContent content = new StringContent(jsonHtmlBody, Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/marketplaces" + id, content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
