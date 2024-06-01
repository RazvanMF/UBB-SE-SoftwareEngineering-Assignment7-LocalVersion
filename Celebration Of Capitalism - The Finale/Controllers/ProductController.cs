using Celebration_Of_Capitalism___The_Finale.Models;
using Celebration_Of_Capitalism___The_Finale.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Celebration_Of_Capitalism___The_Finale.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService productService;

        public ProductController(IProductService productService)
        {
            this.productService = productService;
        }

        /**
         * Shows the Views/[your controller name]/Index.cshtml page
         * Here Views/Product/Index.cshtml
         */
        public IActionResult Index()
        {
            return View();
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
