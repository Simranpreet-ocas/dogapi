using DogApi.Models.Breeds;

namespace DogApi.Endpoints.Breeds
{
    public class RandomBreedImageEndpoint : EndpointWithoutRequest<RandomBreedImageResponse>
    {
        private readonly HttpClient _httpClient;

        public RandomBreedImageEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override void Configure()
        {
            Get("dogs/random-breed-image");
            Policies("Authenticated");
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            // Fetch random breed image from Dog API
            var response = await _httpClient.GetStringAsync("https://dog.ceo/api/breeds/image/random");

            var dogApiResponse = JsonSerializer.Deserialize<RandomBreedImageResponse>(response);
            if (dogApiResponse == null || dogApiResponse.Message == null)
            {
                throw new Exception("Failed to fetch random breed image from Dog API.");
            }

            // Send the random image as a response
            await SendAsync(dogApiResponse);
        }
    }
}
