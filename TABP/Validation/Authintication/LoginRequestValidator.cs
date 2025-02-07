using Application.DTOs.Users;
using FluentValidation;
using TABP.Utilites.ValidationRulesExtension;

namespace TABP.Validation.Authintication
{
    public class LoginRequestValidator : AbstractValidator<LoginRequest>
    {
        public LoginRequestValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required")
                .EmailAddress().WithMessage("Email is not valid");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Password is required")
                .StrongPassword();
        }
    }
}
