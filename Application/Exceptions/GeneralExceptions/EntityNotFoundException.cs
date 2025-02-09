
using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.GeneralExceptions
{
    public class EntityNotFoundException(string message) : NotFoundExceptions(message)
    {
        public override string Header => $"{message} not found";
    }
}
