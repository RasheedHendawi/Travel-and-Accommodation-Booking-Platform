

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class CannotDeleteHotelException(string message) : BadRequestException(message)
    {
        public override string Header => "Cannot delete hotel";
    }
}
