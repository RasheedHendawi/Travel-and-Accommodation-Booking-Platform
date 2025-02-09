

namespace Application.Exceptions.UserExceptions
{
    public class UserNotFoundException : ExceptionsBase
    {
        public UserNotFoundException() : base("User not found.") { }
    }
}
