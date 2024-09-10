using System.Text.Json.Serialization;

namespace DogApi.Models.Breeds
{
    public class DogApiResponse
    {
        [JsonPropertyName("message")]
        public Dictionary<string, List<string>> Message { get; set; }

        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
