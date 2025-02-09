
namespace Application.Exceptions.RoomExceptions
{
    public class DuplicateRoomNumberException : ExceptionsBase
    {
        public DuplicateRoomNumberException()
            : base("Room with this number already exists in the room class.") { }
    }
}
