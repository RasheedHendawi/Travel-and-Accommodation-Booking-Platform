

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class CityWithPostofficeException(string message) : ConflictExceptions(message)
    {
        public override string Header => "City with postoffice";
    }
}
