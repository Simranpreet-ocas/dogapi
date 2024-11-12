using Microsoft.AspNetCore.Mvc;

namespace DogApi.Models.Breeds
{
    public class RandomBreedImageByBreedRequest
    {
        public string Breed { get; set; }
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public int Count { get; set; } = 1;
        public string? Filter { get; set; }
    }
}
