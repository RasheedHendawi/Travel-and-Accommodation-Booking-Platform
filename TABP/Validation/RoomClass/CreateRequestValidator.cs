using Application.DTOs.RoomClass;
using FluentValidation;
using static Domain.Rules.RoomClassRules;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.RoomClass
{
    public class CreateRequestValidator : AbstractValidator<RoomClassCreationRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Description).MaximumLength(TextGeneralSize);
            RuleFor(x => x.AdultCapacity)
                .InclusiveBetween(MinAdults, MaxAdults);
            RuleFor(x => x.ChildCapacity)
                .InclusiveBetween(MinChildren, MaxChildren);
            RuleFor(x => x.Price).GreaterThan(0).NotEmpty();
            RuleFor(x => x.AmenitiesId).NotEmpty();
        }
    }
}
