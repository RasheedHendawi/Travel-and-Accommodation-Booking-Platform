using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.RoomExceptions
{
    public class DuplicateRoomNumberException(string message) : ConflictExceptions(message)
    {
        public override string Header => "Duplicate room number";
    }
}
