using DogApi.Endpoints.Breeds.Models;

namespace DogApi.Endpoints.Breeds
{
    public class RandomBreedImageEndpoint : EndpointWithoutRequest<RandomBreedImageResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RandomBreedImageEndpoint> _logger;

        public RandomBreedImageEndpoint(HttpClient httpClient, ILogger<RandomBreedImageEndpoint> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public override void Configure()
        {
            Get("dogs/random-breed-image");
            Policies("Authenticated");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            _logger.LogInformation("Fetching random breed image from the Dog API");

            try
            {
                // Fetch random breed image from Dog API
                var response = await _httpClient.GetStringAsync("https://dog.ceo/api/breeds/image/random");

                var dogApiResponse = JsonSerializer.Deserialize<RandomBreedImageResponse>(response);
                if (dogApiResponse == null || dogApiResponse.Message == null)
                {
                    _logger.LogError("Failed to fetch random breed image from Dog API");
                    throw new Exception("Failed to fetch random breed image from Dog API.");
                }
                _logger.LogInformation("Successfully fetched random breed image");

                // Send the random image as a response
                await SendAsync(dogApiResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch random breed image from the Dog API: {Message}", ex.Message);
                throw new Exception("Failed to fetch random breed image from the Dog API");
            }
        }
    }
}
