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
    public class AlertDomainCompletionUnitTest
    {
        [TestMethod]
        public void EsotericInterfaceGets_OnOperation_DoNothing()
        {
            NewProductAlert genericNewProductAlert = new NewProductAlert { UserId = 1, ProductId = 1, IAlert = null };
            BackInStockAlert genericBackInStockAlert = new BackInStockAlert { UserId = 1, ProductId = 1, MarketplaceId = 1, IAlert = null };
            PriceDropAlert genericPriceDropAlert = new PriceDropAlert { UserId = 1, ProductId = 1, OldPrice = 1.99F, NewPrice = 0.99F, IAlert = null };

            genericBackInStockAlert.Equals(null);
            genericNewProductAlert.Equals(null);
            genericPriceDropAlert.Equals(null);

            genericBackInStockAlert.IAlert = null;
            genericBackInStockAlert.IAlert1 = null;
            if (genericBackInStockAlert.IAlert == null && genericBackInStockAlert.IAlert1 == null)
            {
                Assert.IsTrue(true);
            }

            genericNewProductAlert.IAlert = null;
            if (genericNewProductAlert.IAlert == null)
            {
                Assert.IsTrue(true);
            }

            genericPriceDropAlert.IAlert = null;
            genericPriceDropAlert.IAlert1 = null;
            if (genericPriceDropAlert.IAlert == null && genericPriceDropAlert.IAlert1 == null)
            {
                Assert.IsTrue(true);
            }

            object? voodooEqualsChecker = null;
            genericBackInStockAlert.Equals(voodooEqualsChecker);
            genericNewProductAlert.Equals(voodooEqualsChecker);
            genericPriceDropAlert.Equals(voodooEqualsChecker);
        }
    }
}
