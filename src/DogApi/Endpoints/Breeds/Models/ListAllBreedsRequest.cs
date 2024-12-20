using Microsoft.AspNetCore.Mvc;

namespace DogApi.Endpoints.Breeds.Models
{
    public class ListAllBreedsRequest
    {
        [QueryParam]
        public int? Page { get; set; }  // Default page = 1
        [QueryParam]
        public int? PageSize { get; set; }  // Default page size =
        [QueryParam]
        public string? Search { get; set; }  // Search query
    }
}
