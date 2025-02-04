namespace DogApi.Endpoints.RandomBreedImageByBreed
{
    /// <summary>
    /// Endpoint to fetch random images by breed.
    /// </summary>
    public class RandomBreedImageByBreedEndpoint : Endpoint<RandomBreedImageByBreedRequest, RandomBreedImageByBreedResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<RandomBreedImageByBreedEndpoint> _logger;
        private readonly IFlagsmithClient _flagsmithClient;
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomBreedImageByBreedEndpoint"/> class.
        /// </summary>
        /// <param name="httpClient">The HTTP client.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="flagsmithClient">The Flagsmith client.</param>
        public RandomBreedImageByBreedEndpoint(HttpClient httpClient, ILogger<RandomBreedImageByBreedEndpoint> logger, IFlagsmithClient flagsmithClient)
        {
            _httpClient = httpClient;
            _logger = logger;
            _flagsmithClient = flagsmithClient;
        }

        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("/dogs/random-breed-image-by-breed");
            Policies("Authenticated");
            Validator<RandomBreedImageByBreedValidator>();

            Summary(s =>
            {
                s.Summary = "Retrieve random images by breed";
                s.Description = "This endpoint fetches a specified number of random images for a given dog breed from the Dog API. " +
                                "You can optionally specify pagination parameters (page number and page size) and a filter term to narrow down the results. " +
                                "The endpoint requires authentication.";
                s.ExampleRequest = new RandomBreedImageByBreedRequest
                {
                    Breed = "retriever",
                    Page = 1,
                    PageSize = 10,
                    Count = 5,
                    Filter = "golden"
                };
                s.ResponseExamples[200] = new RandomBreedImageByBreedResponse
                {
                    Message = new List<string>
                    {
                        "https://images.dog.ceo/breeds/retriever-golden/n02099601_1003.jpg",
                        "https://images.dog.ceo/breeds/retriever-golden/n02099601_1007.jpg"
                    },
                    Status = "success"
                };
                s.Responses[200] = "A list of random breed images";
                s.Responses[400] = "Validation failed";
                s.Responses[401] = "Unauthorized access";
                s.Responses[403] = "Forbidden access";
            });
        }

        /// <summary>
        /// Handles the request to fetch random images by breed.
        /// </summary>
        /// <param name="req">The request model.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
        public override async Task HandleAsync(RandomBreedImageByBreedRequest req, CancellationToken ct)
        {
            var isFeatureEnabled = (await _flagsmithClient.GetEnvironmentFlags()).IsFeatureEnabled("enable_random_breed_image_by_breed_endpoint");

            if (!isFeatureEnabled.Result)
            {
                _logger.LogWarning("Feature flag 'enable_random_breed_image_by_breed_endpoint' is disabled");
                await SendForbiddenAsync();
                return;
            }
            var page = req.Page ?? 1; // Default page = 1
            var pageSize = req.PageSize ?? 10; // Default page size = 10

            _logger.LogInformation("Fetching random images by breed from the Dog API");

            try
            {
                // Fetch the random images by breed
                var response = await _httpClient.GetStringAsync($"https://dog.ceo/api/breed/{req.Breed}/images/random/{req.Count}");

                var dogApiResponse = JsonSerializer.Deserialize<RandomBreedImageByBreedResponse>(response);
                if (dogApiResponse == null || dogApiResponse.Message == null)
                {
                    _logger.LogError("Failed to fetch random images for breed: {Breed}", req.Breed);
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
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                _logger.LogInformation("Returning {Count} images for breed: {Breed}", paginatedImages.Count, req.Breed);

                // Return the filtered and paginated list of images
                await SendAsync(new RandomBreedImageByBreedResponse
                {
                    Status = dogApiResponse.Status,
                    Message = paginatedImages
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching random images for breed: {Breed}", req.Breed);
                throw new Exception("Failed to fetch random images by breed from the Dog API");
            }
        }
    }
}
