

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.BookingExceptions
{
    public class BookingNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "Booking not found";
    }
}
