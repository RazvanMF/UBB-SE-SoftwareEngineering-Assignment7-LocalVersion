using System.Diagnostics;
using System.Data;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using Microsoft.Data.SqlClient;

// CONTROLLER -> SERVICE -> REPOSITORY ARE CASCADING, TESTING THE TOP WILL GO ALL THE WAY TO THE BOTTOM, EXCEPT FOR 2 FUNCTIONS THAT ARE REPO-ONLY,
// AND USED BY THE REPO ITSELF, THEREFORE, ALSO TESTED
namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class ProductControllerUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM Product; DBCC CHECKIDENT('Product', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            SqlCommand addProductCommand = connection.CreateCommand();
            addProductCommand.CommandType = CommandType.Text;
            addProductCommand.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
            addProductCommand.Parameters.AddWithValue("@name", "Nintendo Dog");
            addProductCommand.Parameters.AddWithValue("@category", "animals");
            addProductCommand.Parameters.AddWithValue("@description", "Dog playing frisbee in Wuhu Island");
            addProductCommand.Parameters.AddWithValue("@brand", "Nintendo");
            addProductCommand.Parameters.AddWithValue("@imageUrl", "https://static.wikia.nocookie.net/nintendo/images/0/07/Unimpressed_Dog.png/revision/latest/scale-to-width-down/188?cb=20191030124533&path-prefix=en");
            addProductCommand.Parameters.AddWithValue("@attributes", "colour:beige;size:small;frisbee:not included");
            addProductCommand.ExecuteNonQuery();
            addProductCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }

        [TestMethod]
        public void TestAddProduct_SuccessfulAdd_ReturnsProductID()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            Product productToAdd = new Product
            {
                Name = "addMeProduct",
                Category = "generics",
                Description = "insert lorem here...",
                Brand = "Generic Dispensers PFA",
                ImageURL = "https://i0.wp.com/www.thewebdesigncompany.eu/wp-content/uploads/2020/04/WHAT-IS-lorem-ipsum.jpg?fit=960%2C720&ssl=1",
                Attributes = new Dictionary<string, string> { { "isGeneric", "yes" } }
            };

            int addedProductID = productController.AddProduct(productToAdd);
            Debug.Assert(addedProductID == 2);
            Product? retrievedProduct = productController.GetProduct(2);
            Debug.Assert(retrievedProduct != null
                && retrievedProduct.Name == "addMeProduct"
                && retrievedProduct.Category == "generics"
                && retrievedProduct.Description == "insert lorem here..."
                && retrievedProduct.Brand == "Generic Dispensers PFA"
                && retrievedProduct.ImageURL == "https://i0.wp.com/www.thewebdesigncompany.eu/wp-content/uploads/2020/04/WHAT-IS-lorem-ipsum.jpg?fit=960%2C720&ssl=1"
                && retrievedProduct.Attributes.ContainsKey("isGeneric") == true && retrievedProduct.Attributes["isGeneric"] == "yes");
        }

        [TestMethod]
        public void TestGetProduct_SuccessfulGet_ReturnsProduct()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            int idToFind = 1;
            Product? productFound = productController.GetProduct(idToFind);
            Debug.Assert(productFound != null);
            Debug.Assert(productFound.Name == "Nintendo Dog");
        }

        [TestMethod]
        public void TestGetProduct_FailureGet_ReturnsNull()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            int idToFind = 5;
            Product? productFound = productController.GetProduct(idToFind);
            Debug.Assert(productFound == null);
        }

        [TestMethod]
        public void TestDeleteProduct_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            int idToDelete = 1;
            bool deleteState = productController.DeleteProduct(idToDelete);
            Assert.IsTrue(deleteState);
            Debug.Assert(productController.GetProduct(1) == null);
        }

        [TestMethod]
        public void TestDeleteProduct_FailureDelete_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            int idToDelete = 5;
            bool deleteState = productController.DeleteProduct(idToDelete);
            Assert.IsFalse(deleteState);
        }

        [TestMethod]
        public void TestUpdateProduct_SuccessfulUpdate_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            Product productEntryToBeModifiedInto = new Product
            {
                Name = "Nintendo Mii Matt",
                Category = "Opponent",
                Description = "Top Notch Boxer and Swordplayer",
                Brand = "Nintendo",
                ImageURL = "https://static1.thegamerimages.com/wordpress/wp-content/uploads/2022/02/MiiMatt.png",
                Attributes = new Dictionary<string, string> { { "should not be for sale", "yes" }, { "successfully updated", "yes" } }
            };

            int idOfProductToBeUpdated = 1;
            bool updateState = productController.UpdateProduct(idOfProductToBeUpdated, productEntryToBeModifiedInto);
            Assert.IsTrue(updateState);
            Product? retrievedProduct = productController.GetProduct(idOfProductToBeUpdated);
            Debug.Assert(retrievedProduct != null
                && retrievedProduct.Name == "Nintendo Mii Matt"
                && retrievedProduct.Category == "Opponent"
                && retrievedProduct.Description == "Top Notch Boxer and Swordplayer"
                && retrievedProduct.Brand == "Nintendo"
                && retrievedProduct.ImageURL == "https://static1.thegamerimages.com/wordpress/wp-content/uploads/2022/02/MiiMatt.png"
                && retrievedProduct.Attributes.ContainsKey("successfully updated") == true && retrievedProduct.Attributes["successfully updated"] == "yes");
        }

        [TestMethod]
        public void TestUpdateProduct_FailureUpdate_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            Product productEntryToBeModifiedInto = new Product
            {
                Name = "Nintendo Mii Matt",
                Category = "Opponent",
                Description = "Top Notch Boxer and Swordplayer",
                Brand = "Nintendo",
                ImageURL = "https://static1.thegamerimages.com/wordpress/wp-content/uploads/2022/02/MiiMatt.png",
                Attributes = new Dictionary<string, string> { { "should not be for sale", "yes" }, { "successfully updated", "no" } }
            };

            int idOfProductToBeUpdated = 5;
            bool updateState = productController.UpdateProduct(idOfProductToBeUpdated, productEntryToBeModifiedInto);
            Assert.IsFalse(updateState);
        }

        [TestMethod]
        public void TestGetAllProducts_SuccessfulGet_ReturnsAllProducts()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IProductRepository productRepository = new ProductRepository(configurationManager);
            IProductService productService = new ProductService(productRepository);
            ProductController productController = new ProductController(productService);

            List<Product> products = productController.GetAllProducts().ToList();
            Debug.Assert(products.Count == 1);
            Debug.Assert(products[0].Id == 1 && products[0].Name == "Nintendo Dog");
        }

        [ClassCleanup]
        public static void RestoreProductData()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM Product; DBCC CHECKIDENT('Product', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            // Entry 1
            using (SqlCommand addProductCommand1 = connection.CreateCommand())
            {
                addProductCommand1.CommandType = CommandType.Text;
                addProductCommand1.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand1.Parameters.AddWithValue("@name", "samsung a40");
                addProductCommand1.Parameters.AddWithValue("@category", "phones");
                addProductCommand1.Parameters.AddWithValue("@description", "nice phone");
                addProductCommand1.Parameters.AddWithValue("@brand", "samsung");
                addProductCommand1.Parameters.AddWithValue("@imageUrl", "https://s13emagst.akamaized.net/products/20881/20880622/images/res_381d8711510238431d78e9122a8c18d3.jpg");
                addProductCommand1.Parameters.AddWithValue("@attributes", "colour:blue;memory:128GB");
                addProductCommand1.ExecuteNonQuery();
            }

            // Entry 2
            using (SqlCommand addProductCommand2 = connection.CreateCommand())
            {
                addProductCommand2.CommandType = CommandType.Text;
                addProductCommand2.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand2.Parameters.AddWithValue("@name", "GTB1756VK");
                addProductCommand2.Parameters.AddWithValue("@category", "car parts");
                addProductCommand2.Parameters.AddWithValue("@description", "A good turbo-upgrade for the 1.9 TDI");
                addProductCommand2.Parameters.AddWithValue("@brand", "Garett");
                addProductCommand2.Parameters.AddWithValue("@imageUrl", "https://forums.tdiclub.com/proxy.php?image=http%3A%2F%2Fwww.darkside-developments.co.uk%2F%2Fimages%2Fproducts%2FGTB%2520%281%29.JPG&hash=60187d69a88c934a008304453cc8b48b");
                addProductCommand2.Parameters.AddWithValue("@attributes", "colour:gray;targetHP:200,lag:minimal");
                addProductCommand2.ExecuteNonQuery();
            }

            // Entry 3
            using (SqlCommand addProductCommand3 = connection.CreateCommand())
            {
                addProductCommand3.CommandType = CommandType.Text;
                addProductCommand3.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand3.Parameters.AddWithValue("@name", "Fridge");
                addProductCommand3.Parameters.AddWithValue("@category", "Appliances");
                addProductCommand3.Parameters.AddWithValue("@description", "White 2 metres tall");
                addProductCommand3.Parameters.AddWithValue("@brand", "Beko");
                addProductCommand3.Parameters.AddWithValue("@imageUrl", "https://www.bigchill.uk/Images/Products/UK-Retro-Fridge/RetroEuroFridge-White-Md.jpg");
                addProductCommand3.Parameters.AddWithValue("@attributes", "colour:white");
                addProductCommand3.ExecuteNonQuery();
            }

            // Entry 4
            using (SqlCommand addProductCommand4 = connection.CreateCommand())
            {
                addProductCommand4.CommandType = CommandType.Text;
                addProductCommand4.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand4.Parameters.AddWithValue("@name", "Iphone");
                addProductCommand4.Parameters.AddWithValue("@category", "phones");
                addProductCommand4.Parameters.AddWithValue("@description", "nice phone");
                addProductCommand4.Parameters.AddWithValue("@brand", "Apple");
                addProductCommand4.Parameters.AddWithValue("@imageUrl", "https://cdn.cs.1worldsync.com/2c/5f/2c5fa58c-4955-4d9e-a2c1-7c4ae9f03574.jpg");
                addProductCommand4.Parameters.AddWithValue("@attributes", "colour:black");
                addProductCommand4.ExecuteNonQuery();
            }

            // Entry 5
            using (SqlCommand addProductCommand5 = connection.CreateCommand())
            {
                addProductCommand5.CommandType = CommandType.Text;
                addProductCommand5.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand5.Parameters.AddWithValue("@name", "Iphone12");
                addProductCommand5.Parameters.AddWithValue("@category", "phones");
                addProductCommand5.Parameters.AddWithValue("@description", "nice phone2");
                addProductCommand5.Parameters.AddWithValue("@brand", "Apple");
                addProductCommand5.Parameters.AddWithValue("@imageUrl", "https://images-cdn.ubuy.co.in/64b12805d5e6be5f96724809-apple-iphone-12-mini-64gb-128gb-256gb.jpg");
                addProductCommand5.Parameters.AddWithValue("@attributes", "colour:rainbow");
                addProductCommand5.ExecuteNonQuery();
            }

            // Entry 6
            using (SqlCommand addProductCommand6 = connection.CreateCommand())
            {
                addProductCommand6.CommandType = CommandType.Text;
                addProductCommand6.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand6.Parameters.AddWithValue("@name", "Mouse");
                addProductCommand6.Parameters.AddWithValue("@category", "Computer Accessories");
                addProductCommand6.Parameters.AddWithValue("@description", "Office mouse suitable for anyone");
                addProductCommand6.Parameters.AddWithValue("@brand", "lenovo");
                addProductCommand6.Parameters.AddWithValue("@imageUrl", "https://s1.cel.ro/images/mari/2022/08/09/Lenovo-4Y50X88822-mouse-uri-Ambidextru-Bluetooth-Optice-2400-DPI.jpg");
                addProductCommand6.Parameters.AddWithValue("@attributes", "colour:black;size:slim;type:dual");
                addProductCommand6.ExecuteNonQuery();
            }

            // Entry 7
            using (SqlCommand addProductCommand7 = connection.CreateCommand())
            {
                addProductCommand7.CommandType = CommandType.Text;
                addProductCommand7.CommandText = "INSERT INTO Product (name, category, description, brand, imageUrl, attributes) " +
                    "VALUES (@name, @category, @description, @brand, @imageUrl, @attributes);";
                addProductCommand7.Parameters.AddWithValue("@name", "Keyboard");
                addProductCommand7.Parameters.AddWithValue("@category", "Computer Accessories");
                addProductCommand7.Parameters.AddWithValue("@description", "Office keyboard suitable for anyone");
                addProductCommand7.Parameters.AddWithValue("@brand", "lenovo");
                addProductCommand7.Parameters.AddWithValue("@imageUrl", "https://m.media-amazon.com/images/I/918e33SZeKL.jpg");
                addProductCommand7.Parameters.AddWithValue("@attributes", "type:qwerty;colour:black");
                addProductCommand7.ExecuteNonQuery();
            }

            connection.Close();
            connection.Dispose();
        }
    }
}
