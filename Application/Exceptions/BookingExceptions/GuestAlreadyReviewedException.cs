

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.BookingExceptions
{
    public class GuestAlreadyReviewedException(string message) : ConflictExceptions(message)
    {
        public override string Header => "Guest already reviewed";
    }
}
