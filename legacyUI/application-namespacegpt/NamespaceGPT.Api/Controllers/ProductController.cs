using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;

namespace NamespaceGPT.Api.Controllers
{
    public class ProductController
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        public IEnumerable<Product> GetAllProducts()
        {
            return productService.GetAllProducts();
        }

        public int AddProduct(Product product)
        {
            return productService.AddProduct(product);
        }

        public bool DeleteProduct(int id)
        {
            return productService.DeleteProduct(id);
        }

        public Product? GetProduct(int id)
        {
            return productService.GetProduct(id);
        }

        public bool UpdateProduct(int id, Product product)
        {
            return productService.UpdateProduct(id, product);
        }
    }
}
