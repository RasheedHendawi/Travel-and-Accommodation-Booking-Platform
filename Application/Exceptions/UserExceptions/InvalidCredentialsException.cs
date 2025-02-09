

namespace Application.Exceptions.UserExceptions
{
    public class InvalidCredentialsException : ExceptionsBase
    {
        public InvalidCredentialsException() : base("Credentials are not valid.") { }
    }
}
