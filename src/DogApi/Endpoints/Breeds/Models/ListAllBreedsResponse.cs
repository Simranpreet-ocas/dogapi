namespace DogApi.Endpoints.Breeds.Models
{
    public class ListAllBreedsResponse
    {
        public List<string> Breeds { get; set; }
        public int TotalCount { get; set; }
    }
}
