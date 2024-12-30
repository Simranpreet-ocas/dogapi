namespace DogApi.Endpoints.Authentication.Models
{
    /// <summary>
    /// Represents a user.
    /// </summary>
    public class User
    {
        /// <summary>
        /// Username of the user.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Hashed password of the user.
        /// </summary>
        public string PasswordHash { get; set; }

        /// <summary>
        /// Salt used to hash the password.
        /// </summary>
        public string Salt { get; set; }

        /// <summary>
        /// Role of the user.
        /// </summary>
        public string Role { get; set; }
    }
}
