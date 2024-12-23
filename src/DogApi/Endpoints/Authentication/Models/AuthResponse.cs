namespace DogApi.Endpoints.Authentication.Models
{
    public class AuthResponse
    {
        public string Token { get; set; }
        public DateTime ExpirationMinutes { get; set; }
    }
}
