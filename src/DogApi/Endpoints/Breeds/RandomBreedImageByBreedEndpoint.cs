using DogApi.Endpoints.Breeds.Models;

namespace DogApi.Endpoints.Breeds
{
    public class RandomBreedImageByBreedEndpoint : Endpoint<RandomBreedImageByBreedRequest, RandomBreedImageByBreedResponse>
    {
        private readonly HttpClient _httpClient;

        public RandomBreedImageByBreedEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override void Configure()
        {
            Get("/dogs/random-breed-image/{breed}");
            Policies("Authenticated");
        }

        public override async Task HandleAsync(RandomBreedImageByBreedRequest req, CancellationToken ct)
        {
            // Fetch the random images by breed
            var response = await _httpClient.GetStringAsync($"https://dog.ceo/api/breed/{req.Breed}/images/random/{req.Count}");

            var dogApiResponse = JsonSerializer.Deserialize<RandomBreedImageByBreedResponse>(response);
            if (dogApiResponse == null || dogApiResponse.Message == null)
            {
                throw new Exception($"Failed to fetch random images for breed: {req.Breed}.");
            }

            // Optional filtering logic based on the filter value
            var filteredImages = dogApiResponse.Message;
            if (!string.IsNullOrEmpty(req.Filter))
            {
                filteredImages = filteredImages.Where(img => img.Contains(req.Filter)).ToList();
            }

            // Paging logic
            var paginatedImages = filteredImages
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();

            // Return the filtered and paginated list of images
            await SendAsync(new RandomBreedImageByBreedResponse
            {
                Status = dogApiResponse.Status,
                Message = paginatedImages
            });
        }
    }
}
