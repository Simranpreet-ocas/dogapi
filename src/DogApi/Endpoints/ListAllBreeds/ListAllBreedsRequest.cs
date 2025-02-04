namespace DogApi.Endpoints.ListAllBreeds
{
    /// <summary>
    /// Represents a request to list all breeds.
    /// </summary>
    public class ListAllBreedsRequest
    {
        /// <summary>
        /// Page number to retrieve
        /// </summary>
        [QueryParam]
        public int? Page { get; set; }

        /// <summary>
        /// Page size to limit the number of breeds returned
        /// </summary>
        [QueryParam]
        public int? PageSize { get; set; }

        /// <summary>
        /// Search term to further filter the breeds
        /// </summary>
        [QueryParam]
        public string? Search { get; set; }
    }
}
