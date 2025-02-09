

namespace Application.Exceptions.ExceptionTypes
{
    public class ConflictExceptions(string message) : ExceptionsBase(message)
    {
        public override string Header => "Conflict";
    }
}
