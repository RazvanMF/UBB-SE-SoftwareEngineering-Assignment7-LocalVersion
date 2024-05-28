using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Data.Repositories;
using NamespaceGPT.Data.Models;

// MEANT ONLY TO TEST CASES THAT AREN'T COVERED BY THE CONTROLLER TEST
namespace NamespaceGPT.Tests.Domains
{
    [TestClass]
    public class ListingDomainCompletionUnitTest
    {
        [TestMethod]
        public void EsotericGetterAndSetter_OnOperation_DoNothing()
        {
            Listing listingToAdd = new Listing
            {
                ProductId = 2,
                MarketplaceId = 1,
                Price = 150
            };

            listingToAdd.Marketplace = null;
            listingToAdd.Product = null;
            Assert.IsNull(listingToAdd.Marketplace);
            Assert.IsNull(listingToAdd.Product);
        }
    }
}
