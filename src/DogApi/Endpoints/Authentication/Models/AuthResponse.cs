namespace DogApi.Endpoints.Authentication.Models
{
    /// <summary>
    /// Represents the authentication response model.
    /// </summary>
    public class AuthResponse
    {
        /// <summary>
        /// Gets or sets the generated JWT token.
        /// </summary>
        public string Token { get; set; }

        /// <summary>
        /// Gets or sets the expiration time of the token in minutes.
        /// </summary>
        public DateTime ExpirationMinutes { get; set; }
    }
}
