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
    public class ReviewDomainCompletionUnitTest
    {
        [TestMethod]
        public void EsotericUserGetterAndSetter_OnOperation_DoNothing()
        {
            Review review = new Review
            {
                ProductId = 1,
                UserId = 2,
                Title = "Very Nice",
                Description = "This product is very nice",
                Rating = 5
            };

            review.User = null;
            Debug.Assert(review.User == null);
        }
    }
}
