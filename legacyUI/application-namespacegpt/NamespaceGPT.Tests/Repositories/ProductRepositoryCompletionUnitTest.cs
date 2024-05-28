using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using NamespaceGPT.Common.ConfigurationManager;
using NamespaceGPT.Data.Repositories.Interfaces;
using NamespaceGPT.Data.Repositories;

// MEANT ONLY TO TEST CASES THAT AREN'T COVERED BY THE CONTROLLER TEST
namespace NamespaceGPT.Tests.Repositories
{
    [TestClass]
    public class ProductRepositoryCompletionUnitTest
    {
        [TestMethod]
        public void TestConversionFromStringToDict_OnSuccess_ReturnDict()
        {
            Dictionary<string, string> dictionaryValues = ProductRepository.ConvertAttributesFromStringToDict("should_succeed:true").ToDictionary();
            Debug.Assert(dictionaryValues.Count == 1 && dictionaryValues.ContainsKey("should_succeed") && dictionaryValues["should_succeed"] == "true");
        }

        [TestMethod]
        public void TestConversionFromStringToDict_OnFailure_ThrowException()
        {
            Dictionary<string, string> dictionaryValues = new Dictionary<string, string>();

            try
            {
                dictionaryValues = ProductRepository.ConvertAttributesFromStringToDict(string.Empty).ToDictionary();
                Assert.Fail();
            }
            catch (ArgumentException exception)
            {
                Debug.Assert(exception.Message == "string_attributes cannot be null or empty");
            }
        }

        [TestMethod]
        public void TestConversionFromDictToString_OnSuccess_ReturnString()
        {
            Dictionary<string, string> attributes = new Dictionary<string, string> { { "should not be for sale", "yes" }, { "successfully updated", "no" } };
            string result = ProductRepository.ConvertAttributesFromDictToString(attributes);
            Debug.Assert(result == "should not be for sale:yes;successfully updated:no;");
        }
    }
}
