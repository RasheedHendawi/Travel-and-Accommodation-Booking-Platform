using FluentValidation;

namespace Infrastructure.Authentication.JWT.Validator
{
    public class JwtSettingsValidator : AbstractValidator<JwtSettings>
    {
        public JwtSettingsValidator()
        {
            RuleFor(x => x.Key).NotEmpty();
            RuleFor(x => x.Issuer).NotEmpty();
            RuleFor(x => x.Audience).NotEmpty();
            RuleFor(x => x.Lifetime).GreaterThan(0).GreaterThanOrEqualTo(1);
        }   
    }
}
