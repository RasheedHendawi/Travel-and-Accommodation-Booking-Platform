using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.HotelExceptions
{
    public class AmenityWithNameFoundException(string message) : ConflictExceptions(message)
    {
        public override string Header => "Amenity with name found";
    }
}
