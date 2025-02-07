using Application.DTOs.Rooms;
using FluentValidation;

namespace TABP.Validation.Room
{
    public class GuestGetRequestValidator : AbstractValidator<RoomsForGuestsGetRequest>
    {
        public GuestGetRequestValidator()
        {
            RuleFor(x => x.CheckIn)
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));

            RuleFor(x => x.CheckOut)
                .GreaterThanOrEqualTo(x => x.CheckIn);
        }
    }
}
