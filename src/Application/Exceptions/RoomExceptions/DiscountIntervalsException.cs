using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.RoomExceptions
{
    public class DiscountIntervalsException(string message) : BadRequestException(message)
    {
        public override string Header => "Discount Intervals Exception";
    }
}
