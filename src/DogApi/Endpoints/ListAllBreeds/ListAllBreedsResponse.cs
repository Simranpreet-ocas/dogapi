namespace DogApi.Endpoints.ListAllBreeds
{
    /// <summary>
    /// Represents the response containing a list of all breeds with a total count.
    /// </summary>
    public class ListAllBreedsResponse
    {
        /// <summary>
        /// Gets or sets the list of breeds.
        /// </summary>
        public List<string> Breeds { get; set; }

        /// <summary>
        /// Gets or sets the total count of breeds.
        /// </summary>
        public int TotalCount { get; set; }
    }
}
