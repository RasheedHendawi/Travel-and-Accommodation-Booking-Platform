using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class CityNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "City not found";
    }
}
