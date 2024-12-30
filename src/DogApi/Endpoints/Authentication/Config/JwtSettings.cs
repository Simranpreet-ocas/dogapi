namespace DogApi.Endpoints.Authentication.Config
{
    /// <summary>
    /// Represents the JWT settings.
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Gets or sets the secret key used for signing the JWT token.
        /// </summary>
        public string Secret { get; set; }

        /// <summary>
        /// Gets or sets the issuer of the JWT token.
        /// </summary>
        public string Issuer { get; set; }

        /// <summary>
        /// Gets or sets the audience of the JWT token.
        /// </summary>
        public string Audience { get; set; }

        /// <summary>
        /// Gets or sets the expiration time of the JWT token in minutes.
        /// </summary>
        public int ExpirationMinutes { get; set; }
    }
}
