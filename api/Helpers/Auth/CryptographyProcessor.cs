
using System.Security.Cryptography;
using System.Text;

namespace Helpers.Auth
{
    public static class CryptographyProcessor
    {
        public static string CreateSalt(int size)
        {
            byte[] salt = new byte[size];
            RandomNumberGenerator.Fill(salt);
            return Convert.ToBase64String(salt);
        }
        public static string GenerateHash(string input, string salt)
        {
            byte[] bytes = Encoding.UTF8.GetBytes(input+salt);
            using(var sha256 = SHA256.Create())
            {
                byte[] hash = sha256.ComputeHash(bytes);
                return Convert.ToBase64String(hash);
            }
        }
        public static bool AreEqual(string plainTextInput, string hashedInput, string salt)
        {
            string newHashedInput = GenerateHash(plainTextInput,salt);
            return newHashedInput.Equals(hashedInput);
        }
    }
}