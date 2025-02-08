using Application.DTOs.RoomClass;
using FluentValidation;
using static Domain.Rules.RoomClassRules;
using static Domain.Rules.SharedRules;
using TABP.Utilites.ValidationRulesExtension;
namespace TABP.Validation.RoomClass
{
    public class UpdateRequestValidator : AbstractValidator<RoomClassUpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty()
                .ValidName(MinNameSize, MaxNameSize);
            RuleFor(x => x.Description)
                .MaximumLength(TextGeneralSize);
            RuleFor(x => x.AdultCapacity)
                .InclusiveBetween(MinAdults, MaxAdults);
            RuleFor(x => x.ChildCapacity)
                .InclusiveBetween(MinChildren, MaxChildren);
            RuleFor(x => x.Price)
                .GreaterThan(0)
                .NotEmpty();
        }
    }
}
