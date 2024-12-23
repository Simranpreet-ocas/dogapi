using System.Text.Json.Serialization;

namespace DogApi.Endpoints.Breeds.Models
{
    public class RandomBreedImageResponse
    {
        [JsonPropertyName("message")]
        public string Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
