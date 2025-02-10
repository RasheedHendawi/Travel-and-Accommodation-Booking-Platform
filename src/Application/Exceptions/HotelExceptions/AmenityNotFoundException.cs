using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class AmenityNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "Amenity not found";
    }
}
