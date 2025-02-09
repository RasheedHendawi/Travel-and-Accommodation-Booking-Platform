

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.UserExceptions
{
    public class DuplicateUserException(string message) : ConflictExceptions(message)
    {
        public override string Header => "Duplicate User Exception";
    }
}
