using System.Diagnostics;
using System.Data;
using NamespaceGPT.Common.Modules.Encryption.Module;

namespace NamespaceGPT.Tests.Modules
{
    [TestClass]
    public class ReviewControllerUnitTests
    {
        [TestInitialize]
        public void Initialize()
        {
            return; // good...?
        }

        [TestMethod]
        public void TestSetCypherAlphabet_SuccessfulSet_ThrowsNothing()
        {
            try
            {
                EncryptionModule.SetCypherAlphabet("ZABCDEFGHIJKLMNOPQRSTUVWXY");
            }
            catch (Exception ex)
            {
                Debug.Assert(true == false);
            }
            Debug.Assert(true);
        }

        [TestMethod]
        public void TestSetCypherAlphabet_FailureSetLength_ThrowsError()
        {
            try
            {
                EncryptionModule.SetCypherAlphabet("ZABCDEFGHIJKLMNO");
            }
            catch (Exception ex)
            {
                Debug.Assert(true);
                return;
            }
            Debug.Assert(true == false);
        }

        [TestMethod]
        public void TestSetCypherAlphabet_FailureSetDuplicates_ThrowsError()
        {
            try
            {
                EncryptionModule.SetCypherAlphabet("AABCDEFGHIJKLMNOPQRSTUVWXY");
            }
            catch (Exception ex)
            {
                Debug.Assert(true);
                return;
            }
            Debug.Assert(true == false);
        }

        [TestMethod]
        public void TestSetCypherAlphabet_FailureSetNotAlphabetic_ThrowsError()
        {
            try
            {
                EncryptionModule.SetCypherAlphabet("9ABCDEFGHIJKLMNOPQRSTUVWXY");
            }
            catch (Exception ex)
            {
                Debug.Assert(true);
                return;
            }
            Debug.Assert(true == false);
        }

        [TestMethod]
        public void TestEncrypt_SuccessfulEncrypt_ReturnsEncryptedString()
        {
            EncryptionModule.SetCypherAlphabet("ZBCDEFGHIJKLMNOPQRSTUVWXYA"); // just swap Z and A for my sanity
            string input = "APPLE";
            string result = EncryptionModule.Encrypt(input);

            Assert.IsTrue(result == "ZPPLE");
        }

        [TestMethod]
        public void TestEncrypt_SuccessfulDecrypt_ReturnsDecryptedString()
        {
            EncryptionModule.SetCypherAlphabet("ZBCDEFGHIJKLMNOPQRSTUVWXYA");
            string input = "ZPPLE";
            string result = EncryptionModule.Decrypt(input);

            Assert.IsTrue(result == "APPLE");
        }

        [TestMethod]
        public void TestEncrypt_CompletionFunction_CheckExtraBranches()
        {
            EncryptionModule.SetCypherAlphabet("ZBCDEFGHIJKLMNOPQRSTUVWXYA");
            string input = "APPLE7";
            string result = EncryptionModule.Encrypt(input);

            Assert.IsTrue(result == "ZPPLE7");

            result = EncryptionModule.Decrypt(input);
        }
    }
}
