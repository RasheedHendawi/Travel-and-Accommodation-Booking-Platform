using Application.DTOs.Bookings;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Booking
{
    public class CreateRequestValidator : AbstractValidator<BookingCreationRequest>
    {
        public CreateRequestValidator() 
        { 
            RuleFor(x => x.RoomIds)
                .NotEmpty();
            RuleFor(x => x.HotelId)
                .NotEmpty();
            RuleFor(x => x.CheckIn)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
            RuleFor(x => x.CheckOut)
                .NotEmpty()
                .GreaterThanOrEqualTo(b => b.CheckIn);
            RuleFor(x => x.GuestRemarks)
                .MaximumLength(TextGeneralSize);
            RuleFor(x => x.PaymentMethod)
                .NotEmpty()
                .IsInEnum();
        }
    }
}
