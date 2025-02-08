using FluentValidation;

namespace TABP.Utilites.ValidationRulesExtension
{
    public static class ValidatorExtensions
    {
        public static IRuleBuilderOptions<T, string> ValidName<T>(
          this IRuleBuilder<T, string> ruleBuilder, int minLength, int maxLength)
        {
            return ruleBuilder
              .Matches(@"^[A-Za-z(),\-\s]+$")
              .WithMessage("It should only contain letters.")
              .Length(minLength, maxLength);
        }

        public static IRuleBuilderOptions<T, string> PhoneNumber<T>(
          this IRuleBuilder<T, string> ruleBuilder)
        {
            return ruleBuilder
              .Matches(@"^[\+]?[(]?[0-9]{3}[)]?[-\s\.]?[0-9]{3}[-\s\.]?[0-9]{4,6}$")
              .WithMessage("It must be a valid phone number");
        }

        public static IRuleBuilderOptions<T, string> ValidNumericString<T>(
          this IRuleBuilder<T, string> ruleBuilder,
          int length)
        {
            return ruleBuilder
            .Matches($"^[0-9]{{{length}}}$")
              .WithMessage($"Must be exactly {length}-digits.");
        }

        public static IRuleBuilderOptions<T, IFormFile> ValidImage<T>(
          this IRuleBuilder<T, IFormFile> ruleBuilder)
        {
            var allowedImageTypes = new[] { "image/jpg", "image/jpeg", "image/png" };

            return ruleBuilder
              .Must(x => allowedImageTypes.Contains(x.ContentType, StringComparer.OrdinalIgnoreCase))
              .WithMessage("Not a valid image format, the valid are { png, jpeg, jpg}");
        }

        public static IRuleBuilderOptions<T, string> StrongPassword<T>(this IRuleBuilderOptions<T, string> ruleBuilder)
        {
            return ruleBuilder
              .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[^\da-zA-Z]).{8,}$")
              .WithMessage("Password must contains at least one upper, lower letters and one digit and min lenght of 8 characters.");
        }
    }
}
