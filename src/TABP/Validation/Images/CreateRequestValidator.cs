using Application.DTOs.Images;
using FluentValidation;
using TABP.Utilites.ValidationRulesExtension;

namespace TABP.Validation.Images
{
    public class CreateRequestValidator : AbstractValidator<ImageCreationRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.Image)
                .NotNull()
                .ValidImage();
        }
    }
}
