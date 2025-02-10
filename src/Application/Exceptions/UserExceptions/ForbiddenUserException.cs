

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.UserExceptions
{
    public class ForbiddenUserException(string message) : ForbiddenException(message)
    {
        public override string Header => "Not Guest !";
    }
}
