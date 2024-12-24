namespace DogApi.Endpoints.Breeds.Models
{
    public class ListAllBreedsRequest
    {
        [QueryParam]
        public int? Page { get; set; } = 1;  // Default page = 1
        [QueryParam]
        public int? PageSize { get; set; } = 10; // Default page size =
        [QueryParam]
        public string? Search { get; set; }  // Search query
    }
}
