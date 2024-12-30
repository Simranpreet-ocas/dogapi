using DogApi.Endpoints.Authentication.Models;
using DogApi.Endpoints.Authentication.Utils;

namespace DogApi.Endpoints.Authentication.Services
{
    /// <summary>
    /// Represents the user store.
    /// </summary>
    public class UserStore
    {
        private readonly List<User> _users;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserStore"/> class.
        /// </summary>
        /// <param name="env">The web host environment.</param>
        public UserStore(IWebHostEnvironment env)
        {
            var filepath = Path.Combine(env.ContentRootPath, "Data", "user.json");

            if (File.Exists(filepath))
            {
                var jsonData = File.ReadAllText(filepath);
                _users = JsonSerializer.Deserialize<List<User>>(jsonData) ?? new List<User>();
            }
            else
            {
                _users = new List<User>();
            }
        }

        /// <summary>
        /// Validates the user credentials.
        /// </summary>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <returns>The validated user, or null if validation fails.</returns>
        public User ValidateUser(string username, string password)
        {
            var user = _users.FirstOrDefault(u => u.Username == username);
            if (user == null)
                return null;

            if (PasswordHasher.VerifyPassword(password, user.PasswordHash, user.Salt))
            {
                return user; // Return the authenticated user
            }

            return null; // Return null if password doesn't match
        }
    }
}
