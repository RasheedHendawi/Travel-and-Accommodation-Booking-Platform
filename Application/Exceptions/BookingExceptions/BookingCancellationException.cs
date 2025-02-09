

namespace Application.Exceptions.BookingExceptions
{
    public class BookingCancellationException : ExceptionsBase
    {
        public BookingCancellationException() : base("Cannot cancel booking.") { }
    }
}
