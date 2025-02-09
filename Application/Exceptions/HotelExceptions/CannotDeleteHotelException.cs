

namespace Application.Exceptions.HotelExceptions
{
    public class CannotDeleteHotelException : ExceptionsBase
    {
        public CannotDeleteHotelException()
            : base("Cannot delete hotel with existing room classes.") { }
    }
}
