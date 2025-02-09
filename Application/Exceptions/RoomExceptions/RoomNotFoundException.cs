
namespace Application.Exceptions.RoomExceptions
{
    public class RoomNotFoundException : ExceptionsBase
    {
        public RoomNotFoundException() : base("Room not found.") { }
    }
}
