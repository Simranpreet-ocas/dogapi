namespace DogApi.Models.Breeds
{
    public class ListAllBreedsResponse
    {
        public List<string> Breeds { get; set; }
        public int TotalCount { get; set; }
    }
}
