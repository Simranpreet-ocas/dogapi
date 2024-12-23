using System.Security.Cryptography;
using System.Text;

namespace DogApi.Endpoints.Authentication.Utils
{
    public class PasswordHasher
    {
        // Generates a SHA-256 hash with a salt
        public static string HashPassword(string password, out string salt)
        {
            // Generate a random salt
            byte[] saltBytes = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(saltBytes);
            }
            salt = Convert.ToBase64String(saltBytes);

            // Combine password and salt and compute hash
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(password + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                return Convert.ToBase64String(hashBytes);
            }
        }

        // Verifies a password against a stored hash and salt
        public static bool VerifyPassword(string enteredPassword, string storedHash, string salt)
        {
            using (var sha256 = SHA256.Create())
            {
                byte[] combinedBytes = Encoding.UTF8.GetBytes(enteredPassword + salt);
                byte[] hashBytes = sha256.ComputeHash(combinedBytes);
                string enteredHash = Convert.ToBase64String(hashBytes);
                return storedHash == enteredHash;
            }
        }
    }
}
