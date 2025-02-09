

namespace Application.Exceptions.ExceptionTypes
{
    public class BadRequestException(string message) : ExceptionsBase(message)
    {
        public override string Header => "Bad Request";
    }
}
