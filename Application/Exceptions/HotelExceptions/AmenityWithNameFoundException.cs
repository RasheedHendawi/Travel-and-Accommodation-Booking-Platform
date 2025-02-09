

namespace Application.Exceptions.HotelExceptions
{
    public class AmenityWithNameFoundException : ExceptionsBase
    {
        public AmenityWithNameFoundException() : base("Amenity with this name already exists.")
        {
        }
    }
}
