

namespace Application.Exceptions.UserExceptions
{
    public class DuplicateUserException : ExceptionsBase
    {
        public DuplicateUserException() : base("User with this email already exists.") { }
    }
}
