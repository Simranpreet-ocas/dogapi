using System.Text.Json.Serialization;

namespace DogApi.Endpoints.Breeds.Models
{
    /// <summary>
    /// Represents the response from the Dog API.
    /// </summary>
    public class DogApiResponse
    {
        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        /// <example>success</example>
        [JsonPropertyName("status")]
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message containing the breeds and their sub-breeds.
        /// </summary>
        [JsonPropertyName("message")]
        public Dictionary<string, List<string>> Message { get; set; }
    }
}
