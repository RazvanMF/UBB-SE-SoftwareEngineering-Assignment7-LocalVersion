using System.Text;
using System.Xml.Linq;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Repositories.Interfaces;
using Newtonsoft.Json;

namespace NamespaceGPT.Business.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository productRepository;

        public ProductService(IProductRepository productRepository)
        {
            this.productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        }

        public int AddProduct(Product product)
        {
            try
            {
                string attributesAsString = ProductRepository.ConvertAttributesFromDictToString(product.Attributes);
                ProductToPostable productToPostable = new ProductToPostable { Id = product.Id, Attributes = attributesAsString, Brand = product.Brand, Category = product.Category, Description = product.Description, ImageURL = product.ImageURL, Name = product.Name };
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(productToPostable), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PostAsync("https://localhost:7040/api/products", content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                ProductToPostable? result = JsonConvert.DeserializeObject<ProductToPostable>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                Product returned = new Product { Id = result.Id, Brand = result.Brand, Category = result.Category, Description = result.Description, Name = result.Name, ImageURL = result.ImageURL, Attributes = ProductRepository.ConvertAttributesFromStringToDict(result.Attributes) };

                return returned.Id;
            }
            catch
            {
                return -1;
            }

            // return productRepository.AddProduct(product);
        }

        public bool DeleteProduct(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.DeleteAsync("https://localhost:7040/api/products/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }

            // return productRepository.DeleteProduct(id);
        }

        public IEnumerable<Product> GetAllProducts()
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/products")).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                List<ProductToPostable>? result = JsonConvert.DeserializeObject<List<ProductToPostable>>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }

                List<Product> returned = new List<Product>();
                result.ForEach(element => returned.Add(new Product { Id = element.Id, Brand = element.Brand, Category = element.Category, Description = element.Description, Name = element.Name, ImageURL = element.ImageURL, Attributes = ProductRepository.ConvertAttributesFromStringToDict(element.Attributes) }));

                return returned;
            }
            catch
            {
                return null;
            }

            // return productRepository.GetAllProducts();
        }

        public Product? GetProduct(int id)
        {
            try
            {
                HttpClient client = new HttpClient();
                HttpResponseMessage response = Task.Run(() => client.GetAsync("https://localhost:7040/api/products/" + id)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                string responseBody = Task.Run(() => response.Content.ReadAsStringAsync()).GetAwaiter().GetResult();
                ProductToPostable? result = JsonConvert.DeserializeObject<ProductToPostable>(responseBody);
                if (result == null)
                {
                    throw new Exception("???");
                }
                Product returned = new Product { Id = result.Id, Brand = result.Brand, Category = result.Category, Description = result.Description, Name = result.Name, ImageURL = result.ImageURL, Attributes = ProductRepository.ConvertAttributesFromStringToDict(result.Attributes) };
                return returned;
            }
            catch
            {
                return null;
            }

            // return productRepository.GetProduct(id);
        }

        public bool UpdateProduct(int id, Product product)
        {
            try
            {
                string attributesAsString = ProductRepository.ConvertAttributesFromDictToString(product.Attributes);
                ProductToPostable productToPostable = new ProductToPostable { Id = product.Id, Attributes = attributesAsString, Brand = product.Brand, Category = product.Category, Description = product.Description, ImageURL = product.ImageURL, Name = product.Name };
                HttpClient client = new HttpClient();
                StringContent content = new StringContent(JsonConvert.SerializeObject(productToPostable), Encoding.UTF8, "application/json");

                HttpResponseMessage response = Task.Run(() => client.PutAsync("https://localhost:7040/api/products" + id, content)).GetAwaiter().GetResult();
                response.EnsureSuccessStatusCode();  // error-prone
                return true;
            }
            catch
            {
                return false;
            }

            // return productRepository.UpdateProduct(id, product);
        }
    }
}
