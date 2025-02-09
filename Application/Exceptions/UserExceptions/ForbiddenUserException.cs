

namespace Application.Exceptions.UserExceptions
{
    public class ForbiddenUserException : ExceptionsBase
    {
        public ForbiddenUserException() : base("Forbidden User (Not Guest).") { }
    }
}
