using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Repositories.Interfaces;

namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class MarketplaceControllerUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM Marketplace; DBCC CHECKIDENT('Marketplace', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();
            SqlCommand addMarketplaceCommand = connection.CreateCommand();
            addMarketplaceCommand.CommandType = CommandType.Text;
            addMarketplaceCommand.CommandText = "INSERT INTO Marketplace (marketplacename, websiteurl, country) " +
                "VALUES (@marketplacename, @websiteurl, @country);";
            addMarketplaceCommand.Parameters.AddWithValue("@marketplacename", "Amazon");
            addMarketplaceCommand.Parameters.AddWithValue("@websiteurl", "www.amazon.com");
            addMarketplaceCommand.Parameters.AddWithValue("@country", "USA");
            addMarketplaceCommand.ExecuteNonQuery();
            addMarketplaceCommand.Dispose();

            connection.Close();
            connection.Dispose();
        }

        [TestMethod]
        public void TestAddMarketplace_SuccessfulAdd_ReturnsMarketplaceID()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            Marketplace marketplaceToAdd = new Marketplace
            {
                Name = "Steam",
                WebsiteURL = "https://store.steampowered.com",
                CountryOfOrigin = "USA"
            };

            int addedMatrketplaceID = marketplaceController.AddMarketplace(marketplaceToAdd);
            Debug.Assert(addedMatrketplaceID == 2);
            Marketplace? retrievedMaketplace = marketplaceController.GetMarketplace(2);
            Debug.Assert(
                retrievedMaketplace != null &&
                retrievedMaketplace.Id == 2 &&
                retrievedMaketplace.Name == "Steam" &&
                retrievedMaketplace.WebsiteURL == "https://store.steampowered.com" &&
                retrievedMaketplace.CountryOfOrigin == "USA");
        }

        [TestMethod]
        public void TestGetMarketplace_SuccessfulGet_ReturnsMarketplace()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            int idToFind = 1;
            Marketplace? marketplaceFound = marketplaceController.GetMarketplace(idToFind);
            Debug.Assert(marketplaceFound != null);
            Debug.Assert(marketplaceFound.Name == "Amazon");
        }

        [TestMethod]
        public void TestGetMarketplace_FailureGet_ReturnsNull()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            int idToFind = 1987;
            Marketplace? marketplaceFound = marketplaceController.GetMarketplace(idToFind);
            Debug.Assert(marketplaceFound == null);
        }

        [TestMethod]
        public void TestDeleteMarketplace_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            int idToDelete = 1;
            bool deleteState = marketplaceController.DeleteMarketplace(idToDelete);
            Assert.IsTrue(deleteState);
            Debug.Assert(marketplaceController.GetMarketplace(1) == null);
        }

        [TestMethod]
        public void TestDeleteMarketplace_SuccessfulDelete_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            int idToDelete = 1987;
            bool deleteState = marketplaceController.DeleteMarketplace(idToDelete);
            Assert.IsFalse(deleteState);
        }

        [TestMethod]
        public void TestUpdateMarketplace_SuccessfulUpdate_ReturnsBooleanStateTrue()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            Marketplace marketplaceEntryToBeModifiedInto = new Marketplace
            {
                Name = "Steam",
                WebsiteURL = "https://store.steampowered.com",
                CountryOfOrigin = "USA"
            };

            int idOfMarketplaceToBeUpdated = 1;
            bool updateState = marketplaceController.UpdateMarketplace(idOfMarketplaceToBeUpdated, marketplaceEntryToBeModifiedInto);
            Assert.IsTrue(updateState);
            Marketplace? retrievedMarketplace = marketplaceController.GetMarketplace(idOfMarketplaceToBeUpdated);
            Debug.Assert(
                retrievedMarketplace != null &&
                retrievedMarketplace.Name == "Steam" &&
                retrievedMarketplace.WebsiteURL == "https://store.steampowered.com" &&
                retrievedMarketplace.CountryOfOrigin == "USA");
        }

        [TestMethod]
        public void TestUpdateMarkeplace_FailureUpdate_ReturnsBooleanStateFalse()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            Marketplace marketplaceEntryToBeModifiedInto = new Marketplace
            {
                Name = "Steam",
                WebsiteURL = "https://store.steampowered.com",
                CountryOfOrigin = "USA"
            };

            int idOfMarketplaceToBeUpdated = 1987;
            bool updateState = marketplaceController.UpdateMarketplace(idOfMarketplaceToBeUpdated, marketplaceEntryToBeModifiedInto);
            Assert.IsFalse(updateState);
        }

        [TestMethod]
        public void TestGetAllMarketplace_SuccessfulGet_ReturnsAllMarketplaces()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            IMarketplaceRepository marketplaceRepository = new MarketplaceRepository(configurationManager);
            IMarketplaceService marketplaceService = new MarketplaceService(marketplaceRepository);
            MarketplaceController marketplaceController = new MarketplaceController(marketplaceService);

            List<Marketplace> marketplaces = marketplaceController.GetAllMarketplaces().ToList();
            Debug.Assert(marketplaces.Count == 1);
            Debug.Assert(marketplaces[0].Id == 1 && marketplaces[0].Name == "Amazon");
        }

        [ClassCleanup]
        public static void RestoreMarketplaceData()
        {
            IConfigurationManager configurationManager = new ConfigurationManager();
            using SqlConnection connection = new (configurationManager.GetConnectionString("appsettings.json"));
            connection.Open();

            SqlCommand truncateCommand = connection.CreateCommand();
            truncateCommand.CommandType = CommandType.Text;
            truncateCommand.CommandText = "DELETE FROM Marketplace; DBCC CHECKIDENT('Marketplace', RESEED, 0);";
            truncateCommand.ExecuteNonQuery();
            truncateCommand.Dispose();

            // Entry 1
            using (SqlCommand addMarketplaceCommand1 = connection.CreateCommand())
            {
                addMarketplaceCommand1.CommandType = CommandType.Text;
                addMarketplaceCommand1.CommandText = "INSERT INTO Marketplace (marketplacename, websiteurl, country) " +
                    "VALUES (@marketplacename, @websiteurl, @country);";
                addMarketplaceCommand1.Parameters.AddWithValue("@marketplacename", "Amazon");
                addMarketplaceCommand1.Parameters.AddWithValue("@websiteurl", "...");
                addMarketplaceCommand1.Parameters.AddWithValue("@country", "...");
                addMarketplaceCommand1.ExecuteNonQuery();
            }

            // Entry 2
            using (SqlCommand addMarketplaceCommand2 = connection.CreateCommand())
            {
                addMarketplaceCommand2.CommandType = CommandType.Text;
                addMarketplaceCommand2.CommandText = "INSERT INTO Marketplace (marketplacename, websiteurl, country) " +
                    "VALUES (@marketplacename, @websiteurl, @country);";
                addMarketplaceCommand2.Parameters.AddWithValue("@marketplacename", "Emag");
                addMarketplaceCommand2.Parameters.AddWithValue("@websiteurl", "...");
                addMarketplaceCommand2.Parameters.AddWithValue("@country", "...");
                addMarketplaceCommand2.ExecuteNonQuery();
            }

            // Entry 3
            using (SqlCommand addMarketplaceCommand3 = connection.CreateCommand())
            {
                addMarketplaceCommand3.CommandType = CommandType.Text;
                addMarketplaceCommand3.CommandText = "INSERT INTO Marketplace (marketplacename, websiteurl, country) " +
                    "VALUES (@marketplacename, @websiteurl, @country);";
                addMarketplaceCommand3.Parameters.AddWithValue("@marketplacename", "Ebay");
                addMarketplaceCommand3.Parameters.AddWithValue("@websiteurl", "...");
                addMarketplaceCommand3.Parameters.AddWithValue("@country", "...");
                addMarketplaceCommand3.ExecuteNonQuery();
            }

            connection.Close();
            connection.Dispose();
        }
    }
}
