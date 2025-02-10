using Domain.Enums;

namespace Application.DTOs.Bookings
{
    public class BookingCreationRequest
    {
        public IEnumerable<Guid> RoomIds { get; init; }
        public Guid HotelId { get; init; }
        public DateOnly CheckIn { get; init; }
        public DateOnly CheckOut { get; init; }
        public string? GuestRemarks { get; init; }
        public PaymentMethod PaymentMethod { get; init; }
    }
}
