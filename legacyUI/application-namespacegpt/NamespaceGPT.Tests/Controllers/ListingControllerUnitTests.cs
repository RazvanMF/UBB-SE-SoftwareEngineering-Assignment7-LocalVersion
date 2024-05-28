using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using NamespaceGPT.Api.Controllers;
using NamespaceGPT.Business.Services;
using NamespaceGPT.Business.Services.Interfaces;
using NamespaceGPT.Data.Models;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Common.ConfigurationManager;

namespace NamespaceGPT.UnitTesting.Controllers
{
    [TestClass]
    public class ListingControllerUnitTests
    {
        private IListingService listingService;
        private ListingController listingController;

        [TestInitialize]
        public void Initialize()
        {
            // Initialize the actual ListingService and ListingController
            IConfigurationManager configurationManager = new ConfigurationManager();
            IListingRepository listingRepository = new ListingRepository(configurationManager);
            listingService = new ListingService();
            listingController = new ListingController(listingService);
        }

        [TestMethod]
        public void TestAddListing_SuccessfulAdd_ReturnsListingID()
        {
            // Arrange
            Listing listingToAdd = new Listing
            {
                ProductId = 1,
                MarketplaceId = 1,
                Price = 100
            };

            // Act
            int addedListingID = listingController.Addlisting(listingToAdd);

            // Assert
            Assert.IsTrue(addedListingID > 0); // Assuming added listing ID should be positive
        }

        [TestMethod]
        public void TestDeleteListing_SuccessfulDelete_ReturnsBooleanStateTrue()
        {
            // Arrange
            // Add a listing to work with
            Listing listingToAdd = new Listing
            {
                ProductId = 2,
                MarketplaceId = 1,
                Price = 150
            };
            int addedListingID = listingController.Addlisting(listingToAdd);

            // Act
            bool deleteState = listingController.Deletelisting(addedListingID);

            // Assert
            Assert.IsTrue(deleteState);
        }

        [TestMethod]
        public void TestGetAllListings_SuccessfulGet_ReturnsAllListings()
        {
            // Act
            IEnumerable<Listing> listings = listingController.GetAllListings();

            // Assert
            Assert.IsNotNull(listings);
            // Additional assertions as needed
        }

        [TestMethod]
        public void TestGetAllListingsOfProduct_SuccessfulGet_ReturnsListingsOfProduct()
        {
            // Arrange
            int productId = 1;

            // Act
            IEnumerable<Listing> listings = listingController.GetAllListingsOfProduct(productId);

            // Assert
            Assert.IsNotNull(listings);
            // Add additional assertions to verify if the returned listings belong to the specified product
            Assert.IsTrue(listings.All(listing => listing.ProductId == productId));
        }

        [TestMethod]
        public void TestGetlisting_SuccessfulGet_ReturnsListing()
        {
            // Arrange
            int id = 2;

            // Act
            Listing listing = listingController.Getlisting(id);

            // Assert
            Assert.IsNotNull(listing);
            Assert.AreEqual(id, listing.Id);
        }

        [TestMethod]
        public void TestGetlisting_FailureGet_ReturnsNull()
        {
            // Arrange
            int id = 999;

            // Act
            Listing listing = listingController.Getlisting(id);

            // Assert
            Assert.IsNull(listing);
        }

        [TestMethod]
        public void TestUpdatelisting_SuccessfulUpdate_ReturnsTrue()
        {
            // Arrange
            // Add a listing to work with
            Listing listingToAdd = new Listing
            {
                ProductId = 3,
                MarketplaceId = 1,
                Price = 200
            };
            int addedListingID = listingController.Addlisting(listingToAdd);

            // Act
            Listing listingToUpdate = new Listing
            {
                Id = addedListingID,
                ProductId = 4, // Update with new product ID
                MarketplaceId = 2, // Update with new marketplace ID
                Price = 250 // Update with new price
            };
            bool updateState = listingController.Updatelisting(addedListingID, listingToUpdate);

            // Assert
            Assert.IsTrue(updateState);
            // Optionally, add additional assertions to verify if the listing is updated correctly
            Listing updatedListing = listingController.Getlisting(addedListingID);
            Assert.AreEqual(listingToUpdate.ProductId, updatedListing.ProductId);
            Assert.AreEqual(listingToUpdate.MarketplaceId, updatedListing.MarketplaceId);
            Assert.AreEqual(listingToUpdate.Price, updatedListing.Price);
        }
    }
}