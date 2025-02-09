

namespace Application.Exceptions.BookingExceptions
{
    public class GuestAlreadyReviewedException : ExceptionsBase
    {
        public GuestAlreadyReviewedException() : base("Guest already reviewed this hotel.")
        {
        }
    }
}
