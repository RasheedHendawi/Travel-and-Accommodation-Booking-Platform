

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class DuplicateHotelException(string message) : ConflictExceptions(message)
    {
        public override string Header => "Hotel already exists";
    }
}
