

namespace Application.Exceptions.BookingExceptions
{
    public class RoomNotAvailableException : ExceptionsBase
    {
        public RoomNotAvailableException(Guid roomId)
            : base($"Room not available ({roomId}).") { }
    }
}
