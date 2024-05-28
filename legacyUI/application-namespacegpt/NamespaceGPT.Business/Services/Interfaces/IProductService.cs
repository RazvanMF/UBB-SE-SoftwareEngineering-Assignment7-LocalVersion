using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Business.Services.Interfaces
{
    public interface IProductService
    {
        int AddProduct(Product product);
        bool DeleteProduct(int id);
        bool UpdateProduct(int id, Product product);
        IEnumerable<Product> GetAllProducts();
        Product? GetProduct(int id);
    }
}
