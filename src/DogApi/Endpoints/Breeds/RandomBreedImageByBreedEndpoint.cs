﻿using DogApi.Endpoints.Breeds.Models;
using DogApi.Endpoints.Breeds.Validators;

namespace DogApi.Endpoints.Breeds
{
    public class RandomBreedImageByBreedEndpoint : Endpoint<RandomBreedImageByBreedRequest, RandomBreedImageByBreedResponse>
    {
        private readonly HttpClient _httpClient;

        public RandomBreedImageByBreedEndpoint(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public override void Configure()
        {
            Get("/dogs/random-breed-image-by-breed");
            Policies("Authenticated");
            Validator<RandomBreedImageByBreedValidator>();
        }

        public override async Task HandleAsync(RandomBreedImageByBreedRequest req, CancellationToken ct)
        {
            var page = req.Page ?? 1; // Default page = 1
            var pageSize = req.PageSize ?? 10; // Default page size = 10
            // Fetch the random images by breed
            var response = await _httpClient.GetStringAsync($"https://dog.ceo/api/breed/{req.Breed}/images/random/{req.Count}");

            var dogApiResponse = JsonSerializer.Deserialize<RandomBreedImageByBreedResponse>(response);
            if (dogApiResponse == null || dogApiResponse.Message == null)
            {
                throw new Exception($"Failed to fetch random images for breed: {req.Breed}.");
            }

            // Optional filtering logic based on the filter value
            var filteredImages = dogApiResponse.Message;
            if (!string.IsNullOrEmpty(req.Filter))
            {
                filteredImages = filteredImages.Where(img => img.Contains(req.Filter)).ToList();
            }

            // Paging logic
            var paginatedImages = filteredImages
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            // Return the filtered and paginated list of images
            await SendAsync(new RandomBreedImageByBreedResponse
            {
                Status = dogApiResponse.Status,
                Message = paginatedImages
            });
        }
    }
}
