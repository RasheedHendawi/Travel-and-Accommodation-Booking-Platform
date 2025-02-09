

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.BookingExceptions
{
    public class BookingCancellationException(string message) : ConflictExceptions(message) 
    {
        public override string Header => "Can not delete";
    }
}
