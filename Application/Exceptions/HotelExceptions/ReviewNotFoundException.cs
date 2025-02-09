

namespace Application.Exceptions.HotelExceptions
{
    public class ReviewNotFoundException(string message) : ExceptionsBase(message)
    {
        public override string Header => "Review Not Found";
    }
}
