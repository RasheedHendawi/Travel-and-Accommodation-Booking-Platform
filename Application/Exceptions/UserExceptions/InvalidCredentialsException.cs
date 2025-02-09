

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.UserExceptions
{
    public class InvalidCredentialsException(string message) : ForbiddenException(message)
    {
        public override string Header => "Credentials not valid.";
    }
}
