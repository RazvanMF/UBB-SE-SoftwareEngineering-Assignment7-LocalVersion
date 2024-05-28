using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Data.Repositories.Interfaces
{
    public interface IProductRepository
    {
        int AddProduct(Product product);
        bool DeleteProduct(int id);
        bool UpdateProduct(int id, Product product);
        IEnumerable<Product> GetAllProducts();
        Product? GetProduct(int id);
    }
}
