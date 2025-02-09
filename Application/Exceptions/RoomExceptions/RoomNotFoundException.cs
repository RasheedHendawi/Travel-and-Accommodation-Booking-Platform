
using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.RoomExceptions
{
    public class RoomNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "Room not found.";
    }
}
