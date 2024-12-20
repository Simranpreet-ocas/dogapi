using DogApi.Endpoints.Breeds.Models;

namespace DogApi.Endpoints.Breeds.Validators
{
      public class ListAllBreedsRequestValidator : AbstractValidator<ListAllBreedsRequest>
      {
            public ListAllBreedsRequestValidator()
            {
                RuleFor(x => x.Page)
                    .NotNull()
                    .WithMessage("Page number must be greater than 0");

                RuleFor(x => x.PageSize)
                    .InclusiveBetween(1, 100)
                    .WithMessage("Page size must be between 1 and 100.");

                RuleFor(x => x.Search)
                    .MaximumLength(50)
                    .WithMessage("Search term should not exceed 50 chars");
            }
      }
}
