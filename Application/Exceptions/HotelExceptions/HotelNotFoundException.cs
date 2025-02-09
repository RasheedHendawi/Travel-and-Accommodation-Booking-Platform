

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class HotelNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "Hotel not found";
    }
}
