using System.Security.Cryptography;

namespace DogApi.Endpoints.Authentication.Utils
{
    /// <summary>
    /// Provides methods for hashing and verifying passwords using SHA-256 with a salt.
    /// </summary>
    public class PasswordHasher
    {
        /// <summary>
        /// Generates a SHA-256 hash of the password with a salt.
        /// </summary>
        /// <param name="password">The password to hash.</param>
        /// <param name="salt">The generated salt used for hashing.</param>
        /// <returns>The hashed password as a base64-encoded string.</returns>
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

        /// <summary>
        /// Verifies a password against a stored hash and salt.
        /// </summary>
        /// <param name="enteredPassword">The password entered by the user.</param>
        /// <param name="storedHash">The stored hash to compare against.</param>
        /// <param name="salt">The salt used to hash the stored password.</param>
        /// <returns><c>true</c> if the entered password matches the stored hash; otherwise, <c>false</c>.</returns>
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
