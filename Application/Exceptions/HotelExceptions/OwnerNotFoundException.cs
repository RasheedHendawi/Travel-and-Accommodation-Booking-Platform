

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class OwnerNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "Owner Not Found";
    }
}
