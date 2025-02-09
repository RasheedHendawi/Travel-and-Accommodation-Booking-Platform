

namespace Application.Exceptions.BookingExceptions
{
    public class BookingNotFoundException : ExceptionsBase
    {
        public BookingNotFoundException() : base("Booking not found for the guest.") { }
    }
}
