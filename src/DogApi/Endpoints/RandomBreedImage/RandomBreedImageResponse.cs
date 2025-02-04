using System.Text.Json.Serialization;

namespace DogApi.Endpoints.RandomBreedImage
{
    /// <summary>
    /// Represents the response containing a random breed image.
    /// </summary>
    public class RandomBreedImageResponse
    {
        /// <summary>
        /// Gets or sets the URL of the random breed image.
        /// </summary>
        /// <example>https://images.dog.ceo/breeds/hound-afghan/n02088094_1003.jpg</example>
        [JsonPropertyName("message")]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        /// <example>success</example>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
