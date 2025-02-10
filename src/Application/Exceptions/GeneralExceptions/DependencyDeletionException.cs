

using Application.Exceptions.ExceptionTypes;

namespace Application.Exceptions.GeneralExceptions
{
    public class DependencyDeletionException(string message) : BadRequestException(message)
    {
        public override string Header => "Dependency Deletion";
    }
}
