using DogApi.Endpoints.Authentication.Config;
using DogApi.Endpoints.Authentication.Models;
using DogApi.Endpoints.Authentication.Services;
using DogApi.Endpoints.Breeds.Models;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace DogApi.Endpoints.Authentication
{

    /// <summary>
    /// Endpoint to generate a JWT token for authenticated users.
    /// </summary>
    public class GenerateTokenEndpoint : Endpoint<AuthRequest, AuthResponse>
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserStore _userStore;
        private readonly ILogger<GenerateTokenEndpoint> _logger;
        private readonly IFlagsmithClient _flagsmithClient;

        /// <summary>
        /// Initializes a new instance of the <see cref="GenerateTokenEndpoint"/> class.
        /// </summary>
        /// <param name="jwtSettings">The JWT settings.</param>
        /// <param name="userStore">The user store.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="flagsmithClient">The Flagsmith client.</param>
        public GenerateTokenEndpoint(IOptions<JwtSettings> jwtSettings, UserStore userStore, ILogger<GenerateTokenEndpoint> logger, IFlagsmithClient flagsmithClient)
        {
            _jwtSettings = jwtSettings.Value;
            _userStore = userStore;
            _logger = logger;
            _flagsmithClient = flagsmithClient;
        }

        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        public override void Configure()
        {
            Post("/auth/token");
            AllowAnonymous();

            Summary(s =>
            {
                s.Summary = "Generate a JWT token for authenticated users";
                s.Description = "This endpoint generates a JWT token for authenticated users. " +
                                "The endpoint requires a valid username and password. " +
                                "If the feature flag 'enable_auth' is disabled, the endpoint will return a 403 Forbidden response.";
                s.ResponseExamples[200] = new AuthResponse
                {
                    Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
                    ExpirationMinutes = DateTime.UtcNow.AddMinutes(60)
                };
                s.Responses[200] = "JWT token generated successfully";
                s.Responses[400] = "Validation failed";
                s.Responses[401] = "Unauthorized access";
                s.Responses[403] = "Forbidden access";
                s.Responses[500] = "Internal server error";
            });
        }

        /// <summary>
        /// Handles the request to generate a JWT token.
        /// </summary>
        /// <param name="req">The authentication request model.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task HandleAsync(AuthRequest req, CancellationToken ct)
        {
            var isFeatureEnabled = (await _flagsmithClient.GetEnvironmentFlags()).IsFeatureEnabled("enable_auth");

            if (!isFeatureEnabled.Result)
            {
                _logger.LogWarning("Feature flag 'enable_auth' is disabled");
                await SendForbiddenAsync();
                return;
            }

            _logger.LogInformation("Token generation request received for user: {Username}", req.Username);

            try
            {
                // Retrieve user from the store
                var user = _userStore.ValidateUser(req.Username, req.Password);
                if (user == null)
                {
                    _logger.LogWarning("Invalid login attempt for user: {Username}", req.Username);
                    await SendUnauthorizedAsync();
                    return;
                }

                _logger.LogInformation("User {Username} authenticated successfully", req.Username);

                // Generate JWT Token (only if user is authenticated)
                var tokenHandler = new JwtSecurityTokenHandler();
                var key = Encoding.ASCII.GetBytes(_jwtSettings.Secret);

                var tokenDescriptor = new SecurityTokenDescriptor
                {
                    Subject = new ClaimsIdentity(new[]
                    {
                new Claim(ClaimTypes.Name, user.Username),
                new Claim(ClaimTypes.Role, user.Role)
            }),
                    Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationMinutes),
                    Issuer = _jwtSettings.Issuer,
                    Audience = _jwtSettings.Audience,
                    SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
                };

                var token = tokenHandler.CreateToken(tokenDescriptor);
                var tokenString = tokenHandler.WriteToken(token);

                _logger.LogInformation("Token generated successfully for user: {Username}", req.Username);

                await SendAsync(new AuthResponse { Token = tokenString });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to generate token for user: {Username}", req.Username);
                await SendAsync(new AuthResponse { Token = string.Empty }, 500, ct);
            }
        }
    }
}
