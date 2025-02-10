using Application.DTOs.Cities;
using FluentValidation;

namespace TABP.Validation.Cities
{
    public class TrendingCitiesGetValidator : AbstractValidator<TrendingCityRequest>
    {
        public TrendingCitiesGetValidator()
        {
            RuleFor(x => x.Count)
                .GreaterThan(0)
                .LessThan(20)
                .WithMessage("Count must be between [0,20] cities");
        }
    }
}
