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
            Get("api/random-breed-image");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await _httpClient.GetAsync("https://dog.ceo/api/breeds/image/random", ct);
            var content = await response.Content.ReadAsStringAsync(ct);
            var result = JsonSerializer.Deserialize<RandomBreedImageResponse>(content);

            await SendAsync(result, cancellation: ct);
        }
    }
}
