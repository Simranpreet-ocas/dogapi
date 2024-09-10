using DogApi.Models.Breeds;

namespace DogApi.Endpoints.Breeds
{
    public class RandomBreedImageByBreedEndpoint : Endpoint<RandomBreedImageByBreedRequest, RandomBreedImageResponse>
    {
        private readonly HttpClient _httpClient;

        public RandomBreedImageByBreedEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override void Configure()
        {
            Verbs(Http.GET);
            Routes("/api/dog/random-breed-image/{Breed}");
            AllowAnonymous();
        }

        public override async Task HandleAsync(RandomBreedImageByBreedRequest req, CancellationToken ct)
        {
            // Call the Dog API to get a random image for the specified breed
            var response = await _httpClient.GetStringAsync($"https://dog.ceo/api/breed/{req.Breed}/images/random");

            // Parse the response
            var dogApiResponse = JsonSerializer.Deserialize<RandomBreedImageResponse>(response);

            // Return the image URL
            await SendAsync(dogApiResponse);
        }
    }
}
