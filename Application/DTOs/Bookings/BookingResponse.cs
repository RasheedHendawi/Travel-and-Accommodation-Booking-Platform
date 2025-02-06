

namespace Application.DTOs.Bookings
{
    public class BookingResponse
    {
        public Guid Id { get; init; }
        public string HotelName { get; init; }
        public decimal TotalPrice { get; init; }
        public DateOnly CheckIn { get; init; }
        public DateOnly CheckOut { get; init; }
        public DateOnly BookingDate { get; init; }
    }
}
