

namespace Application.Exceptions.GeneralExceptions
{
    public class DependencyDeletionException : ExceptionsBase
    {
        public DependencyDeletionException(string entityName, string dependencyName)
            : base($"{entityName} has dependencies on {dependencyName}.")
        {
        }
    }
}
