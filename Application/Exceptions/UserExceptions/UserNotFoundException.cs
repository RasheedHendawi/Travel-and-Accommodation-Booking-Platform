

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.UserExceptions
{
    public class UserNotFoundException(string message   ) : NotFoundExceptions(message)
    {
        public override string Header => "Not found user";
    }
}
