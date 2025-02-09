

namespace Application.Exceptions.ExceptionTypes
{
    public class UnauthorizedException(string message) : ExceptionsBase(message)
    {
        public override string Header => "Unauthorized";
    }
}
