namespace DogApi.Endpoints.Authentication.Models
{
    /// <summary>
    /// Represents the authentication request model.
    /// </summary>
    public class AuthRequest
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        public string Password { get; set; }
    }
}
