using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class ListingService : IListingService
    {
        public int AddListing(Listing listing)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(listing), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/listings", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                Listing? result = JsonConvert.DeserializeObject<Listing>(responseBody);
                if (result == null)
                {
                    throw new Exception("Failed to deserialize the listing");
                }

                return result.Id;
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteListing(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync($"https://localhost:7040/api/listings/{id}")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch
            {
                return false;
            }
        }

        public IEnumerable<Listing> GetAllListings()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/listings")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<Listing>? result = JsonConvert.DeserializeObject<List<Listing>>(responseBody);
                if (result == null)
                {
                    throw new Exception("Failed to deserialize the listings");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<Listing> GetAllListingsOfProduct(int productId)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync($"https://localhost:7040/api/listings")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<Listing>? result = JsonConvert.DeserializeObject<List<Listing>>(responseBody);
                if (result == null)
                {
                    throw new Exception("Failed to deserialize the listings");
                }
                return result.Where(element => element.ProductId == productId).ToList();
            }
            catch
            {
                return null;
            }
        }

        public Listing? GetListing(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync($"https://localhost:7040/api/listings/{id}")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                Listing? result = JsonConvert.DeserializeObject<Listing>(responseBody);
                if (result == null)
                {
                    throw new Exception("Failed to deserialize the listing");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public bool UpdateListing(int id, Listing listing)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(listing), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PutAsync($"https://localhost:7040/api/listings/{id}", content)).GetAwaiter().GetResult();
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
