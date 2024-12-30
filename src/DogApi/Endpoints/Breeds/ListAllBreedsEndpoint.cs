using DogApi.Endpoints.Breeds.Models;
using DogApi.Endpoints.Breeds.Validators;

namespace DogApi.Endpoints.Breeds
{
    /// <summary>
    /// Endpoint to fetch all the breeds
    /// </summary>
    public class ListAllBreedsEndpoint : Endpoint<ListAllBreedsRequest, ListAllBreedsResponse>
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<ListAllBreedsEndpoint> _logger;

        public ListAllBreedsEndpoint(HttpClient httpClient, ILogger<ListAllBreedsEndpoint> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        /// <summary>
        /// Configures the endpoint.
        /// </summary>
        public override void Configure()
        {
            Get("/dogs/breeds");
            Policies("AdminOnly");
            Validator<ListAllBreedsRequestValidator>();

            Summary(s =>
            {
                s.Summary = "Retrieve a list of all dog breeds";
                s.Description = "This endpoint retrieves a comprehensive list of all dog breeds available in the Dog API. " +
                                "You can optionally specify pagination parameters (page number and page size) and a search term to filter the breeds. " +
                                "This endpoint is restricted to users with admin privileges.";
                s.ExampleRequest = new ListAllBreedsRequest
                {
                    Page = 1,
                    PageSize = 10,
                    Search = "retriever"
                };
                s.ResponseExamples[200] = new ListAllBreedsResponse
                {
                    Breeds = new List<string> { "retriever", "bulldog" },
                    TotalCount = 2
                };
                s.Responses[200] = "A list of dog breeds";
                s.Responses[400] = "Validation failed";
                s.Responses[401] = "Unauthorized access";
                s.Responses[403] = "Forbidden access";
            });
        }

        /// <summary>
        /// Handles the request to fetch all breeds.
        /// </summary>
        /// <param name="req">The request model.</param>
        /// <param name="ct">The cancellation token.</param>
        /// <returns>A task that represents the asynchronous operation.</returns>
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
