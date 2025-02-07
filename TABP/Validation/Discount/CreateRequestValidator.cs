using Application.DTOs.Discounts;
using FluentValidation;
using static Domain.Rules.DiscountRules;

namespace TABP.Validation.Discount
{
    public class CreateRequestValidator : AbstractValidator<DiscountCreationRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.Percentage)
                .NotEmpty()
                .LessThan(MaxDiscount)
                .GreaterThanOrEqualTo(MinDiscount)
                .WithMessage($"Percentage must be between [{MinDiscount},{MaxDiscount}]");
            RuleFor(x => x.StartDate)
                .LessThan(x => x.EndDate)
                .WithMessage("Start date must be before end date");
        }
    }
}
