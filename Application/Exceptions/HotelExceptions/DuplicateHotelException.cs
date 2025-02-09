

namespace Application.Exceptions.HotelExceptions
{
    public class DuplicateHotelException : ExceptionsBase
    {
        public DuplicateHotelException() : base("A hotel at this location already exists.") { }
    }
}
