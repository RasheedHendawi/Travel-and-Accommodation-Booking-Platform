using Application.DTOs.Users;
using FluentValidation;
using TABP.Utilites.ValidationRulesExtension;

namespace TABP.Validation.Authintication
{
    public class RegisterRequestValidator : AbstractValidator<RegisterRequest>
    {
        public RegisterRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress();
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .StrongPassword();
            RuleFor(x => x.FirstName)
                .NotEmpty().WithMessage("First name is required");
            RuleFor(x => x.LastName)
                .NotEmpty().WithMessage("Last name is required");
        }
    }
}
