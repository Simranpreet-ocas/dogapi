using DogApi.Endpoints.Breeds.Models;

namespace DogApi.Endpoints.Breeds
{
    /// <summary>
    /// Endpoint to fetch a random breed image.
    /// </summary>
    public class RandomBreedImageEndpoint : EndpointWithoutRequest<RandomBreedImageResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RandomBreedImageEndpoint> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RandomBreedImageEndpoint"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        public RandomBreedImageEndpoint(HttpClient httpClient, ILogger<RandomBreedImageEndpoint> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("dogs/random-breed-image");
            Policies("Authenticated");

            Summary(s =>
            {
                s.Summary = "Retrieve a random breed image";
                s.Description = "This endpoint fetches a random image of a dog breed from the Dog API. " +
                                "The endpoint requires authentication and returns the URL of the random breed image.";
                s.ResponseExamples[200] = new RandomBreedImageResponse
                {
                    Message = "https://images.dog.ceo/breeds/hound-afghan/n02088094_1003.jpg",
                    Status = "success"
                };
                s.Responses[200] = "A random breed image URL";
                s.Responses[401] = "Unauthorized access";
                s.Responses[403] = "Forbidden access";
            });
        }


        /// <summary>
        /// Handles the request to fetch a random breed image.
        /// </summary>
        /// <param name="ct">Cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
