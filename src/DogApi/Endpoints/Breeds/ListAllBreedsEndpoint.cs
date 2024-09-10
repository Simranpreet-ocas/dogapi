using DogApi.Models.Breeds;

namespace DogApi.Endpoints.Breeds
{
    public class ListAllBreedsEndpoint : EndpointWithoutRequest<ListAllBreedsResponse>
    {
        private readonly HttpClient _httpClient;

        public ListAllBreedsEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override void Configure()
        {
            Get("/breeds/list-all");
            AllowAnonymous();
        }

        public override async Task HandleAsync(CancellationToken ct)
        {
            var response = await _httpClient.GetStringAsync("https://dog.ceo/api/breeds/list/all");
            var breeds = JsonSerializer.Deserialize<DogApiResponse>(response);
            await SendAsync(new ListAllBreedsResponse { Breeds = breeds.Message.Keys.ToList() });
        }
    }
}
