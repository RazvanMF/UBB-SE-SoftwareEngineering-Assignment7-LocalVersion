using System.Text;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class FavouriteProductService : IFavouriteProductService
    {
        private readonly IFavouriteProductRepository favouriteProductRepository;

        public FavouriteProductService(IFavouriteProductRepository favouriteProductRepository)
        {
            this.favouriteProductRepository = favouriteProductRepository ?? throw new ArgumentNullException(nameof(favouriteProductRepository));
        }

        public int AddFavouriteProduct(FavouriteProduct favouriteProduct)
        {
            try
            {
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(favouriteProduct), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/favouriteproducts", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                FavouriteProduct? result = JsonConvert.DeserializeObject<FavouriteProduct>(responseBody);
                if (result == null)
                {
                    throw new Exception("Add new favourite product failed");
                }

                return result.Id;
            }
            catch
            {
                return -1;
            }
        }

        public bool DeleteFavouriteProductFromUser(FavouriteProduct favouriteProduct)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync("https://localhost:7040/api/favouriteproducts/" + favouriteProduct.Id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public IEnumerable<FavouriteProduct> GetAllFavouriteProducts()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/favouriteproducts")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<FavouriteProduct>? result = JsonConvert.DeserializeObject<List<FavouriteProduct>>(responseBody);
                if (result == null)
                {
                    throw new Exception("Fail at getting all favourite products");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<FavouriteProduct> GetAllFavouriteProductsOfUser(int userId)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/favouriteproducts")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<FavouriteProduct>? listFavouriteProduct = JsonConvert.DeserializeObject<List<FavouriteProduct>>(responseBody);
                if (listFavouriteProduct == null)
                {
                    throw new Exception("Fail at getting all favourite products");
                }
                List<FavouriteProduct> result = listFavouriteProduct.Where(favProduct => favProduct.UserId == userId).ToList();
                return result;
            }
            catch
            {
                return null;
            }
        }

        public IEnumerable<int> GetAllUserIdsWhoMarkedProductAsFavourite(int productId)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/favouriteproducts")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<FavouriteProduct>? listFavouriteProduct = JsonConvert.DeserializeObject<List<FavouriteProduct>>(responseBody);
                if (listFavouriteProduct == null)
                {
                    throw new Exception("Fail at getting all favourite products");
                }
                var result = from favProduct in listFavouriteProduct
                             where favProduct.ProductId == productId
                             select favProduct.UserId;
                return result.ToList();
            }
            catch
            {
                return null;
            }
        }

        public FavouriteProduct? GetFavouriteProduct(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/favouriteproducts/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                FavouriteProduct? result = JsonConvert.DeserializeObject<FavouriteProduct>(responseBody);
                if (result == null)
                {
                    throw new Exception("Failed to get favourite product by id");
                }
                return result;
            }
            catch
            {
                return null;
            }
        }
    }
}