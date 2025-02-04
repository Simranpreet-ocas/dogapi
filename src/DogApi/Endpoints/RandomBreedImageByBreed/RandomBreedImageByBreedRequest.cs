namespace DogApi.Endpoints.RandomBreedImageByBreed
{
    /// <summary>
    /// Represents the request to fetch random images by breed.
    /// </summary>
    public class RandomBreedImageByBreedRequest
    {
        /// <summary>
        /// Breed to fetch random images for
        /// </summary>
        [QueryParam]
        public string? Breed { get; set; }

        /// <summary>
        /// Page number to retrieve
        /// </summary>
        [QueryParam]
        public int? Page { get; set; } = 1;

        /// <summary>
        /// Size of the page to retrieve
        /// </summary>
        [QueryParam]
        public int? PageSize { get; set; } = 10;

        /// <summary>
        /// Total number of random images to fetch
        /// </summary>
        [QueryParam]
        public int? Count { get; set; } = 10;

        /// <summary>
        /// Filter (sub-breeds) to apply to the random images
        /// </summary>
        [QueryParam]
        public string? Filter { get; set; }
    }
}
