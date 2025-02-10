using Application.DTOs.Amenities;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Amenities
{
    public class UpdateRequestValidator : AbstractValidator<AmenityUpdateRequest>
    {
        public UpdateRequestValidator() 
        {
            RuleFor(x => x.Name)
                .MinimumLength(MinNameSize)
                .MaximumLength(MaxNameSize);
            RuleFor(x => x.Description)
                .MaximumLength(TextGeneralSize);
        }
    }
}
