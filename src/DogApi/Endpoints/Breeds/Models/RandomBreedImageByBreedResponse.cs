using System.Text.Json.Serialization;

namespace DogApi.Endpoints.Breeds.Models
{
    public class RandomBreedImageByBreedResponse
    {
        [JsonPropertyName("message")]
        public List<string> Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
