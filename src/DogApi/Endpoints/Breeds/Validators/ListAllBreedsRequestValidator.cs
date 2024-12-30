using DogApi.Endpoints.Breeds.Models;

namespace DogApi.Endpoints.Breeds.Validators
{
    /// <summary>
    /// Validator for <see cref="ListAllBreedsRequest"/>.
    /// </summary>
    public class ListAllBreedsRequestValidator : AbstractValidator<ListAllBreedsRequest>
      {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListAllBreedsRequestValidator"/> class and apply the necessary validation rules.
        /// </summary>
        public ListAllBreedsRequestValidator()
            {
                RuleFor(x => x.Page)
                    .InclusiveBetween(1, 50)
                    .WithMessage("Page must be between 1 and 50.");

                RuleFor(x => x.PageSize)
                    .InclusiveBetween(1, 20)
                    .WithMessage("Page size must be between 1 and 20.");

                RuleFor(x => x.Search)
                    .MaximumLength(50)
                    .WithMessage("Search term should not exceed 50 chars");
            }
      }
}
