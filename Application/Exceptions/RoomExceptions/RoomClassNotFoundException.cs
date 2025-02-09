

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.RoomExceptions
{
    public class RoomClassNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => "Room class not found.";
    }
}
