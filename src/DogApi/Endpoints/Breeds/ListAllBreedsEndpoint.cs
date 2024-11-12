using DogApi.Models.Breeds;

namespace DogApi.Endpoints.Breeds
{
    public class ListAllBreedsEndpoint : Endpoint<ListAllBreedsRequest, ListAllBreedsResponse>
    {
        private readonly HttpClient _httpClient;

        public ListAllBreedsEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override void Configure()
        {
            Get("/dogs/breeds");
            Policies("AdminOnly");
        }

        public override async Task HandleAsync(ListAllBreedsRequest req, CancellationToken ct)
        {
            // Fetch breeds from the Dog API
            var response = await _httpClient.GetStringAsync("https://dog.ceo/api/breeds/list/all");
            var result = JsonSerializer.Deserialize<DogApiResponse>(response);

            var allBreeds = result.Message.Keys.ToList();

            // Apply search and pagination using the request model
            var filteredBreeds = allBreeds
                .Where(b => string.IsNullOrEmpty(req.Search) || b.Contains(req.Search, StringComparison.OrdinalIgnoreCase))
                .Skip((req.Page - 1) * req.PageSize)
                .Take(req.PageSize)
                .ToList();

            await SendAsync(new ListAllBreedsResponse
            {
                Breeds = filteredBreeds,
                TotalCount = allBreeds.Count
            });
        }
    }
}
