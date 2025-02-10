using Application.DTOs.Hotels;
using FluentValidation;
using static Domain.Rules.SharedRules;
using static Domain.Rules.HotelRules;
namespace TABP.Validation.Hotels
{
    public class UpdateRequestValidator : AbstractValidator<HotelUpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.CityId).NotEmpty();
            RuleFor(x => x.OwnerId).NotEmpty();
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.StartRating).InclusiveBetween(MinStars, MaxStars);
            RuleFor(x => x.Longitude).InclusiveBetween(-180, 180);
            RuleFor(x => x.Latitude).InclusiveBetween(-90, 90);
            RuleFor(x => x.BriefDescription).MaximumLength(TextMaxSize);
            RuleFor(x => x.Description).MaximumLength(TextGeneralSize);
            RuleFor(x => x.PhoneNubmer).NotEmpty();
        }
    }
}
