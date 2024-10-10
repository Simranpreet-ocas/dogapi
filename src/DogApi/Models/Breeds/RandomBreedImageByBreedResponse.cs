using Microsoft.AspNetCore.Mvc;
using System.Text.Json.Serialization;

namespace DogApi.Models.Breeds
{
    public class RandomBreedImageByBreedResponse
    {
        [JsonPropertyName("message")]
        public List<string> Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
