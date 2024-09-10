using System.Text.Json.Serialization;

namespace DogApi.Models.Breeds
{
    public class RandomBreedImageResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
