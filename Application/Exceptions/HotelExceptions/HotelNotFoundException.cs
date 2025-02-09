

namespace Application.Exceptions.HotelExceptions
{
    public class HotelNotFoundException : ExceptionsBase
    {
        public HotelNotFoundException() : base("Hotel not found.") { }
    }
}
