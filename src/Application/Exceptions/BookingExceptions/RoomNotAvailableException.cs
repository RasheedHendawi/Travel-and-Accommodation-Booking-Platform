

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.BookingExceptions
{
    public class RoomNotAvailableException(string message) : ConflictExceptions(message)
    {
        public override string Header => "Room not available";
    }
}
