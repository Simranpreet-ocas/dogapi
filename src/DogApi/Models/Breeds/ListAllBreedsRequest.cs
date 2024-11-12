using Microsoft.AspNetCore.Mvc;

namespace DogApi.Models.Breeds
{
    public class ListAllBreedsRequest
    {
        public int Page { get; set; } = 1;  // Default page = 1
        public int PageSize { get; set; } = 10;  // Default page size = 10
        public string? Search { get; set; }  // Search query
    }
}
