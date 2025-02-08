using Application.DTOs.Cities;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Cities
{
    public class UpdateRequestValidator : AbstractValidator<CityUpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.Country)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.PostOffice)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
        }
    }
}
