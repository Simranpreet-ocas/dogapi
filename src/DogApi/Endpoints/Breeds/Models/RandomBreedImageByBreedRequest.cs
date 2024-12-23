using Microsoft.AspNetCore.Mvc;

namespace DogApi.Endpoints.Breeds.Models
{
    public class RandomBreedImageByBreedRequest
    {
        [QueryParam]
        public string? Breed { get; set; }
        [QueryParam]
        public int? Page { get; set; } = 1;
        [QueryParam]
        public int? PageSize { get; set; } = 10;
        [QueryParam]
        public int? Count { get; set; } = 10;
        [QueryParam]    
        public string? Filter { get; set; }
    }
}
