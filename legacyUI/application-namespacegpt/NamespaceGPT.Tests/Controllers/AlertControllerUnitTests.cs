using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Common.ConfigurationManager;

namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class AlertControllerUnitTests
    {
        private int initialAlertCount = 0;
        private User? testUser;
        private Marketplace? testMarketplace;
        private List<IAlert> testAlerts = new List<IAlert>();

        private IConfigurationManager configurationManager = new ConfigurationManager();
        private IAlertRepository? alertRepository;
        private IAlertService? alertService;
        private AlertController? alertController;
        private Product? testProduct;

        [TestInitialize]
        public void Initialize()
        {
            alertRepository = new AlertRepository(configurationManager);
            alertService = new AlertService(alertRepository);
            alertController = new AlertController(alertService);
            ProductController productController = new ProductController(new ProductService(new ProductRepository(configurationManager)));
            UserController userController = new UserController(new UserService(new UserRepository(configurationManager)));
            MarketplaceController marketplaceController = new MarketplaceController(new MarketplaceService(new MarketplaceRepository(configurationManager)));

            testProduct = new Product
            {
                Name = "AlertTestProduct",
                Category = "generics",
                Description = "Product used for testing alerts",
                Brand = "CelebrationOfCapitalism",
                ImageURL = "https://i0.wp.com/www.thewebdesigncompany.eu/wp-content/uploads/2020/04/WHAT-IS-lorem-ipsum.jpg?fit=960%2C720&ssl=1",
                Attributes = new Dictionary<string, string> { { "comments", "no" }, { "foreign_keys", "no" } }
            };

            testUser = new User
            {
                Username = "test_user",
                Password = "test_user"
            };

            testMarketplace = new Marketplace
            {
                Name = "test_marketplace",
                WebsiteURL = "https://www.test.marketplace",
                CountryOfOrigin = "testing_country"
            };

            int addedProductID = productController.AddProduct(testProduct);
            testProduct.Id = addedProductID;

            int addedUserId = userController.AddUser(testUser);
            testUser.Id = addedUserId;

            int addedMarketplaceID = marketplaceController.AddMarketplace(testMarketplace);
            testMarketplace.Id = addedMarketplaceID;

            initialAlertCount = alertRepository.GetAllAlerts().Count();
        }

        [TestMethod]
        public void TestAddAlerts_SuccessfulAdd_ReturnsAlertID()
        {
            Debug.Assert(testProduct != null);
            Debug.Assert(alertController != null);
            Debug.Assert(testUser != null);
            Debug.Assert(testMarketplace != null);

            IAlert priceDropAlert = new PriceDropAlert
            {
                NewPrice = 100,
                OldPrice = 150,
                ProductId = testProduct.Id,
                UserId = testUser.Id,

                // this does nothing afaik
                IAlert = null,
                IAlert1 = null
            };

            IAlert newProductAlert = new NewProductAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                IAlert = null
            };

            IAlert backInStockAlert = new BackInStockAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                MarketplaceId = testMarketplace.Id,
                IAlert = null,
                IAlert1 = null
            };

            int resultID = alertController.AddAlert(backInStockAlert);
            backInStockAlert.Id = resultID;

            resultID = alertController.AddAlert(newProductAlert);
            newProductAlert.Id = resultID;

            resultID = alertController.AddAlert(priceDropAlert);
            priceDropAlert.Id = resultID;

            testAlerts.Add(backInStockAlert);
            testAlerts.Add(newProductAlert);
            testAlerts.Add(priceDropAlert);

            List<IAlert> alerts = alertController.GetAllAlerts().ToList();
            Debug.Assert(alerts.Contains(backInStockAlert));
            Debug.Assert(alerts.Contains(newProductAlert));
            Debug.Assert(alerts.Contains(priceDropAlert));

            foreach (var alert in alerts)
            {
                try
                {
                    alert.Notify();
                }
                catch
                {
                }
            }
        }

        [TestMethod]
        public void TestAddAlert_Failure_ThrowsException()
        {
            try
            {
                alertController.AddAlert(null);
                Debug.Assert(false);
            }
            catch
            {
            }
        }

        [TestMethod]
        public void TestGetAlerts_SuccessfulGet_ReturnsAlerts()
        {
            try
            {
                alertController.GetAlert(42, new BackInStockAlert());
                Debug.Assert(false);
            }
            catch
            {
            }

            List<IAlert> alerts = alertController.GetAllAlerts().ToList();
            Debug.Assert(alerts.Count() > 0);

            foreach (IAlert alert in alerts)
            {
                Debug.Assert(alerts.Contains(alert));
            }
        }

        [TestMethod]
        public void TestDeleteAlert_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            Debug.Assert(alertController != null);
            List<IAlert> deleteAlerts = new List<IAlert>();

            IAlert priceDropAlert = new PriceDropAlert
            {
                NewPrice = 100,
                OldPrice = 150,
                ProductId = testProduct.Id,
                UserId = testUser.Id,

                // this does nothing afaik
                IAlert = null,
                IAlert1 = null
            };

            IAlert newProductAlert = new NewProductAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                IAlert = null
            };

            IAlert backInStockAlert = new BackInStockAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                MarketplaceId = testMarketplace.Id,
                IAlert = null,
                IAlert1 = null
            };

            int resultID = alertController.AddAlert(backInStockAlert);
            backInStockAlert.Id = resultID;

            resultID = alertController.AddAlert(newProductAlert);
            newProductAlert.Id = resultID;

            resultID = alertController.AddAlert(priceDropAlert);
            priceDropAlert.Id = resultID;

            deleteAlerts.Add(backInStockAlert);
            deleteAlerts.Add(newProductAlert);
            deleteAlerts.Add(priceDropAlert);

            foreach (IAlert alert in deleteAlerts)
            {
                int idToDelete = alert.Id;
                bool deleteState = alertController.DeleteAlert(idToDelete, alert);
                Assert.IsTrue(deleteState);
                Debug.Assert(!(alertController.GetAllAlerts().Contains(alert)));
            }
        }

        [TestMethod]
        public void TestDeleteAlert_FailureDelete_ReturnsBooleanStateFalse()
        {
            Debug.Assert(alertController != null);
            int idToDelete = -1;
            bool deleteState = alertController.DeleteAlert(idToDelete, new NewProductAlert());
            Assert.IsFalse(deleteState);

            try
            {
                alertController.DeleteAlert(idToDelete, null);
                Debug.Assert(false);
            }
            catch
            {
            }
        }

        [TestMethod]
        public void TestDeleteAlert_Failure_ThrowsException()
        {
            try
            {
                alertController.DeleteAlert(1, null);
                Debug.Assert(false);
            }
            catch
            {
            }
        }

        [TestMethod]
        public void TestUpdateAlert_SuccessfulUpdate_ReturnsBooleanStateTrue()
        {
            Debug.Assert(alertController != null);
            List<IAlert> deleteAlerts = new List<IAlert>();

            PriceDropAlert priceDropAlert = new PriceDropAlert
            {
                NewPrice = 100,
                OldPrice = 150,
                ProductId = testProduct.Id,
                UserId = testUser.Id,

                // this does nothing afaik
                IAlert = null,
                IAlert1 = null
            };

            NewProductAlert newProductAlert = new NewProductAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                IAlert = null
            };

            BackInStockAlert backInStockAlert = new BackInStockAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                MarketplaceId = testMarketplace.Id,
                IAlert = null,
                IAlert1 = null
            };

            int resultID = alertController.AddAlert(backInStockAlert);
            backInStockAlert.Id = resultID;

            resultID = alertController.AddAlert(newProductAlert);
            newProductAlert.Id = resultID;

            resultID = alertController.AddAlert(priceDropAlert);
            priceDropAlert.Id = resultID;

            deleteAlerts.Add(backInStockAlert);
            deleteAlerts.Add(newProductAlert);
            deleteAlerts.Add(priceDropAlert);

            backInStockAlert.ProductId = 1;
            newProductAlert.ProductId = 1;
            priceDropAlert.NewPrice = priceDropAlert.OldPrice;

            List<IAlert> alerts = alertController.GetAllAlerts().ToList();

            foreach (IAlert alert in deleteAlerts)
            {
                Debug.Assert(!alerts.Contains(alert));
                alertController.UpdateAlert(alert.Id, alert);
            }

            alerts = alertController.GetAllAlerts().ToList();

            foreach (IAlert alert in deleteAlerts)
            {
                Debug.Assert(alerts.Contains(alert));
            }

            foreach (IAlert alert in deleteAlerts)
            {
                int idToDelete = alert.Id;
                bool deleteState = alertController.DeleteAlert(idToDelete, alert);
                Assert.IsTrue(deleteState);
                Debug.Assert(!(alertController.GetAllAlerts().Contains(alert)));
            }
        }

        [TestMethod]
        public void TestUpdateAlert_FailedUpdate_ReturnsBooleanStateFalse()
        {
            Debug.Assert(alertController != null);
            NewProductAlert newProductAlert = new NewProductAlert
            {
                UserId = testUser.Id,
                ProductId = testProduct.Id,
                IAlert = null,
                Id = 0
            };

            Debug.Assert(!alertController.UpdateAlert(newProductAlert.Id, newProductAlert));
        }

        [TestMethod]
        public void TestUpdateAlert_Failure_ThrowsException()
        {
            try
            {
                alertController.UpdateAlert(1, null);
                Debug.Assert(false);
            }
            catch
            {
            }
        }

        [TestMethod]
        public void TestGetAllAlertsByProductId()
        {
            Debug.Assert(alertController != null);
            IAlert genericNewProductAlert = new NewProductAlert { UserId = testUser.Id, ProductId = testProduct.Id };
            IAlert genericBackInStockAlert = new BackInStockAlert { UserId = testUser.Id, ProductId = testProduct.Id, MarketplaceId = 1 };
            IAlert genericPriceDropAlert = new PriceDropAlert { UserId = testUser.Id, ProductId = testProduct.Id, OldPrice = 1.99F, NewPrice = 0.99F };

            int result1ID = alertController.AddAlert(genericNewProductAlert);
            int result2ID = alertController.AddAlert(genericBackInStockAlert);
            int result3ID = alertController.AddAlert(genericPriceDropAlert);
            genericNewProductAlert.Id = result1ID;
            genericBackInStockAlert.Id = result2ID;
            genericPriceDropAlert.Id = result3ID;

            List<IAlert> alerts = alertController.GetAllProductAlerts(testProduct.Id).ToList();

            Debug.Assert(alerts.Contains(genericNewProductAlert));
            Debug.Assert(alerts.Contains(genericBackInStockAlert));
            Debug.Assert(alerts.Contains(genericPriceDropAlert));
            Debug.Assert(alerts.Count() > 0);

            alertController.DeleteAlert(genericNewProductAlert.Id, genericNewProductAlert);
            alertController.DeleteAlert(genericBackInStockAlert.Id, genericBackInStockAlert);
            alertController.DeleteAlert(genericPriceDropAlert.Id, genericPriceDropAlert);
        }

        [TestMethod]
        public void TestGetAllAlertsByUserId()
        {
            Debug.Assert(alertController != null);
            IAlert genericNewProductAlert = new NewProductAlert { UserId = testUser.Id, ProductId = testProduct.Id };
            IAlert genericBackInStockAlert = new BackInStockAlert { UserId = testUser.Id, ProductId = testProduct.Id, MarketplaceId = 1 };
            IAlert genericPriceDropAlert = new PriceDropAlert { UserId = testUser.Id, ProductId = testProduct.Id, OldPrice = 1.99F, NewPrice = 0.99F };

            int result1ID = alertController.AddAlert(genericNewProductAlert);
            int result2ID = alertController.AddAlert(genericBackInStockAlert);
            int result3ID = alertController.AddAlert(genericPriceDropAlert);
            genericNewProductAlert.Id = result1ID;
            genericBackInStockAlert.Id = result2ID;
            genericPriceDropAlert.Id = result3ID;

            List<IAlert> alerts = alertController.GetAllUserAlerts(testUser.Id).ToList();

            Debug.Assert(alerts.Contains(genericNewProductAlert));
            Debug.Assert(alerts.Contains(genericBackInStockAlert));
            Debug.Assert(alerts.Contains(genericPriceDropAlert));
            Debug.Assert(alerts.Count() > 0);

            alertController.DeleteAlert(genericNewProductAlert.Id, genericNewProductAlert);
            alertController.DeleteAlert(genericBackInStockAlert.Id, genericBackInStockAlert);
            alertController.DeleteAlert(genericPriceDropAlert.Id, genericPriceDropAlert);
        }

        [TestCleanup]
        public void RestoreAlertData()
        {
            Debug.Assert(alertController != null);

            foreach (IAlert alert in testAlerts)
            {
                int idToDelete = alert.Id;
                bool deleteState = alertController.DeleteAlert(idToDelete, alert);
                Assert.IsTrue(deleteState);
                Debug.Assert(!(alertController.GetAllAlerts().Contains(alert)));
            }
        }

        [ClassCleanup]
        public static void RestoreEverythingData()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM AppUser; DBCC CHECKIDENT('AppUser', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            SqlCommand addUserCommand = connection.CreateCommand();
            addUserCommand.CommandType = CommandType.Text;
            addUserCommand.CommandText = "INSERT INTO AppUser (username, password) VALUES (@username, @password);";
            addUserCommand.Parameters.AddWithValue("@username", "RazvanMF");
            addUserCommand.Parameters.AddWithValue("@password", "password");
            addUserCommand.ExecuteNonQuery();
            addUserCommand.Dispose();

            SqlCommand truncateProdCommand = connection.CreateCommand();
            truncateProdCommand.CommandType = CommandType.Text;
            truncateProdCommand.CommandText = "DELETE FROM Product; DBCC CHECKIDENT('Product', RESEED, 0);";
            truncateProdCommand.ExecuteNonQuery();
            truncateProdCommand.Dispose();

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