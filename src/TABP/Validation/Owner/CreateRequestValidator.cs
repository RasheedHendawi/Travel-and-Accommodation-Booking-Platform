using Application.DTOs.Owners;
using FluentValidation;
using TABP.Utilites.ValidationRulesExtension;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Owner
{
    public class CreateRequestValidator : AbstractValidator<OwnerCreationRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotNull()
                .EmailAddress();
            RuleFor(x => x.FirstName)
                .NotNull()
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.LastName)
                .NotNull()
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.PhoneNumber)
                .NotNull()
                .PhoneNumber();
        }
    }
}
