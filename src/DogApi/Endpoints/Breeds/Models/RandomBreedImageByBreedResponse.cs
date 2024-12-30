using System.Text.Json.Serialization;

namespace DogApi.Endpoints.Breeds.Models
{
    /// <summary>
    /// Represents the response containing random images by breed.
    /// </summary>
    public class RandomBreedImageByBreedResponse
    {
        /// <summary>
        /// Gets or sets the list of image URLs.
        /// </summary>
        /// <example>
        /// [
        ///   "https://images.dog.ceo/breeds/hound-afghan/n02088094_1003.jpg",
        ///   "https://images.dog.ceo/breeds/hound-afghan/n02088094_1007.jpg"
        /// ]
        /// </example>
        [JsonPropertyName("message")]
        public List<string> Message { get; set; }

        /// <summary>
        /// Gets or sets the status of the response.
        /// </summary>
        /// <example>success</example>
        [JsonPropertyName("status")]
        public string Status { get; set; }
    }
}
