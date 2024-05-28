using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class SaleService : ISaleService
    {
        private readonly ISaleRepository saleRepository;

        public SaleService(ISaleRepository saleRepository)
        {
            this.saleRepository = saleRepository ?? throw new ArgumentNullException(nameof(saleRepository));
        }

        public int AddSale(Sale sale)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(sale), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/sales", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                Sale? result = JsonConvert.DeserializeObject<Sale>(responseBody);
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

        public bool DeleteSale(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync("https://localhost:7040/api/sales/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Sale> GetAllPurchasesOfUser(int userId)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/sales")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<Sale>? listSales = JsonConvert.DeserializeObject<List<Sale>>(responseBody);
                if (listSales == null)
                {
                    throw new Exception("Fail at getting all sales");
                }
                List<Sale> result = listSales.Where(sale => sale.BuyerId == userId).ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Sale> GetAllSales()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/sales")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<Sale>? result = JsonConvert.DeserializeObject<List<Sale>>(responseBody);
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

        public IEnumerable<Sale> GetAllSalesOfListing(int listingId)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync($"https://localhost:7040/api/sales")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<Sale>? result = JsonConvert.DeserializeObject<List<Sale>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                var filteredResult = result.Where(entry => entry.ListingId == listingId).ToList();
                return filteredResult;
            }
            catch
            {
                return null;
            }
        }

        public Sale? GetSale(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync($"https://localhost:7040/api/sales/{id}")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                Sale? result = JsonConvert.DeserializeObject<Sale>(responseBody);
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

        public bool UpdateSale(int id, Sale sale)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(sale), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PutAsync($"https://localhost:7040/api/sales/{id}", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
