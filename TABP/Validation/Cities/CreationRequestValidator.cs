using Application.DTOs.Cities;
using FluentValidation;
using static Domain.Rules.SharedRules;
using static Domain.Rules.CityRules;
using TABP.Utilites.ValidationRulesExtension;
namespace TABP.Validation.Cities
{
    public class CreationRequestValidator : AbstractValidator<CityCreationRequest>
    {
        public CreationRequestValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.Country)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.PostOffice)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize)
                .ValidNumericString(PostOffices);
        }
    }
}
