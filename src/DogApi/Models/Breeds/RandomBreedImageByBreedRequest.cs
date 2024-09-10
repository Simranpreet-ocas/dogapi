using Microsoft.AspNetCore.Mvc;

namespace DogApi.Models.Breeds
{
    public class RandomBreedImageByBreedRequest
    {
        [QueryParam]
        public string Breed { get; set; }
    }
}
