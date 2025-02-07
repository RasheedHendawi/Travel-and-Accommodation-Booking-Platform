using Application.DTOs.Reviews;
using FluentValidation;
using static Domain.Rules.SharedRules;
using static Domain.Rules.ReviewsRules;
namespace TABP.Validation.Review
{
    public class CreateRequestValidator : AbstractValidator<ReviewCreationRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(TextGeneralSize);
            RuleFor(x => x.Rating)
                .InclusiveBetween(MinRating, MaxRating);
        }
    }
}
