using DogApi.Endpoints.Authentication.Config;
using DogApi.Endpoints.Authentication.Models;
using DogApi.Endpoints.Authentication.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace DogApi.Endpoints.Authentication
{
    public class GenerateTokenEndpoint : Endpoint<AuthRequest, AuthResponse>
    {
        private readonly JwtSettings _jwtSettings;
        private readonly UserStore _userStore;

        public GenerateTokenEndpoint(IOptions<JwtSettings> jwtSettings, UserStore userStore)
        {
            _jwtSettings = jwtSettings.Value;
            _userStore = userStore;
        }

        public override void Configure()
        {
            Post("/auth/token");
            AllowAnonymous();
        }

        public override async Task HandleAsync(AuthRequest req, CancellationToken ct)
        {
            // Retrieve user from the store
            var user = _userStore.ValidateUser(req.Username, req.Password);
            if (user == null)
            {
                await SendUnauthorizedAsync();
                return;
            }

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

            await SendAsync(new AuthResponse { Token = tokenString });
        }
    }
}
