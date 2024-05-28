namespace NamespaceGPT.Common.Modules.Encryption.Module
{
    // Module responsible for encryption and decryption of strings
    public static class EncryptionModule
    {
        private static Dictionary<char, char> encryptionDictionary = new Dictionary<char, char>();
        private static Dictionary<char, char> decryptionDictionary = new Dictionary<char, char>();

        private static string originalAlphabet = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";

        static public void SetCypherAlphabet(string encryptedAlphabet)
        {
            encryptionDictionary.Clear();
            decryptionDictionary.Clear();

            if (encryptedAlphabet.Length != 26)
            {
                throw new ArgumentException("Cipher alphabet does not contain 26 letters!");
            }
            if (encryptedAlphabet.Distinct().Count() != 26)
            {
                throw new ArgumentException("All letters in the cipher must be distinct!");
            }
            if (encryptedAlphabet.Any(c => !char.IsLetter(c)))
            {
                throw new ArgumentException("Cipher alphabet must contain only letters.");
            }

            encryptedAlphabet = encryptedAlphabet.ToUpper();

            for (int i = 0; i < encryptedAlphabet.Length; i++)
            {
                encryptionDictionary.Add(originalAlphabet[i], encryptedAlphabet[i]);
                decryptionDictionary.Add(encryptedAlphabet[i], originalAlphabet[i]);

                encryptionDictionary.Add(char.ToLower(originalAlphabet[i]), char.ToLower(encryptedAlphabet[i]));
                decryptionDictionary.Add(char.ToLower(encryptedAlphabet[i]), char.ToLower(originalAlphabet[i]));
            }

            encryptionDictionary.Add(' ', ' ');
            decryptionDictionary.Add(' ', ' ');
        }

        static public string Encrypt(string originalString)
        {
            string encryptedString = string.Empty;
            foreach (char c in originalString)
            {
                if (encryptionDictionary.ContainsKey(c))
                {
                    encryptedString += encryptionDictionary[c];
                }
                else
                {
                    encryptedString += c;
                }
            }

            return encryptedString;
        }

        static public string Decrypt(string encryptedString)
        {
            string decryptedString = string.Empty;
            foreach (char c in encryptedString)
            {
                if (decryptionDictionary.ContainsKey(c))
                {
                    decryptedString += decryptionDictionary[c];
                }
                else
                {
                    decryptedString += c;
                }
            }

            return decryptedString;
        }
    }
}