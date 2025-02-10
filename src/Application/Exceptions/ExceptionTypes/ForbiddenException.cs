

namespace Application.Exceptions.ExceptionTypes
{
    public class ForbiddenException(string message) : ExceptionsBase(message)
    {
        public override string Header => "Forbidden";
    }
}
