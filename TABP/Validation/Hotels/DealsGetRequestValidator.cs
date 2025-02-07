using Application.DTOs.Hotels;
using FluentValidation;

namespace TABP.Validation.Hotels
{
    public class DealsGetRequestValidator : AbstractValidator<FeaturedDealsRequest>
    {
        public DealsGetRequestValidator()
        {
            RuleFor(x => x.Count)
              .InclusiveBetween(1, 100);
        }
    }
}
