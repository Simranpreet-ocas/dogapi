using DogApi.Endpoints.Breeds.Models;
using DogApi.Endpoints.Breeds.Validators;

namespace DogApi.Endpoints.Breeds
{
    public class ListAllBreedsEndpoint : Endpoint<ListAllBreedsRequest, ListAllBreedsResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ListAllBreedsEndpoint> _logger;

        public ListAllBreedsEndpoint(HttpClient httpClient, ILogger<ListAllBreedsEndpoint> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public override void Configure()
        {
            Get("/dogs/breeds");
            Policies("AdminOnly");
            Validator<ListAllBreedsRequestValidator>();
        }

        public override async Task HandleAsync(ListAllBreedsRequest req, CancellationToken ct)
        {
            var page = req.Page ?? 1; // Default page = 1
            var pageSize = req.PageSize ?? 10; // Default page size = 10

            _logger.LogInformation("Fetching breeds from the Dog API");

            try
            {
                // Fetch breeds from the Dog API
                var response = await _httpClient.GetStringAsync("https://dog.ceo/api/breeds/list/all");
                var result = JsonSerializer.Deserialize<DogApiResponse>(response);

                var allBreeds = result.Message.Keys.ToList();

                // Apply search and pagination using the request model
                var filteredBreeds = allBreeds
                    .Where(b => string.IsNullOrEmpty(req.Search) || b.Contains(req.Search, StringComparison.OrdinalIgnoreCase))
                    .Skip((page - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();
                _logger.LogInformation("Successfully fetched and filtered breed");

                await SendAsync(new ListAllBreedsResponse
                {
                    Breeds = filteredBreeds,
                    TotalCount = allBreeds.Count
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to fetch breeds from the Dog API: {Message}", ex.Message);
                throw new Exception("Failed to fetch breeds from the Dog API");
            }
        }
    }
}
