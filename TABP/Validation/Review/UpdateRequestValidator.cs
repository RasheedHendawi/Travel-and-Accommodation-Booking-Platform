using Application.DTOs.Reviews;
using FluentValidation;
using static Domain.Rules.SharedRules;
using static Domain.Rules.ReviewsRules;
namespace TABP.Validation.Review
{
    public class UpdateRequestValidator : AbstractValidator<ReviewUpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.Content)
                .NotEmpty()
                .MaximumLength(TextGeneralSize);
            RuleFor(x => x.Rating)
                .NotEmpty()
                .InclusiveBetween(MinRating, MaxRating);
        }
    }
}
