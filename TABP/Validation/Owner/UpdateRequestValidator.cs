using Application.DTOs.Owners;
using FluentValidation;
using static Domain.Rules.SharedRules;
using TABP.Utilites.ValidationRulesExtension;

namespace TABP.Validation.Owner
{
    public class UpdateRequestValidator : AbstractValidator<OwnerUpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .ValidName(MinNameSize, MaxNameSize);
            RuleFor(x => x.LastName)
                .NotEmpty()
                .ValidName(MinNameSize, MaxNameSize);
            RuleFor(x => x.Email)
                .NotEmpty()
                .EmailAddress();
            RuleFor(x => x.PhoneNumber)
                .NotEmpty()
                .PhoneNumber();
        }
    }
}
