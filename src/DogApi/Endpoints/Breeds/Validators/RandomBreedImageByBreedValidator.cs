using DogApi.Endpoints.Breeds.Models;

namespace DogApi.Endpoints.Breeds.Validators
{
    /// <summary>
    /// Validator for <see cref="RandomBreedImageByBreedRequest"/>.
    /// </summary>
    public class RandomBreedImageByBreedValidator : AbstractValidator<RandomBreedImageByBreedRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RandomBreedImageByBreedValidator"/> class and apply the necessary validation rules.
        /// </summary>
        public RandomBreedImageByBreedValidator()
        {
            RuleFor(x => x.Breed)
                .NotNull()
                .WithMessage("Breed name is required.");
            RuleFor(x => x.Page)
                .InclusiveBetween(1, 20)
                .WithMessage("Page must be between 1 and 20.");
            RuleFor(x => x.PageSize)
                .InclusiveBetween(1, 20)
                .WithMessage("Page size must be between 1 and 20.");
            RuleFor(x => x.Count)
                .InclusiveBetween(1, 100)
                .WithMessage("Count must be between 1 and 100.");
            RuleFor(x => x.Filter)
                .MaximumLength(50)
                .WithMessage("Filter term should not exceed 50 chars");
        }
    }
}
