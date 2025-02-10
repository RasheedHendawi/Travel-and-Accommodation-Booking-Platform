

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.GeneralExceptions
{
    public class ArgumentOutOfRangeException(string message) : BadRequestException(message)
    {
        public override string Header => ($"Argument '{message}' is out of range.");
    }
}
