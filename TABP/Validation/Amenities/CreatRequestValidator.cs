using Application.DTOs.Amenities;
using static Domain.Rules.SharedRules;
using FluentValidation;

namespace TABP.Validation.Amenities
{
    public class CreatRequestValidator : AbstractValidator<AmenityCreationRequest>
    {
        public CreatRequestValidator()
        {
            RuleFor(x => x.Name)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.Description)
                .MaximumLength(TextGeneralSize);
        }
    }
}
