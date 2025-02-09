
namespace Application.Exceptions.GeneralExceptions
{
    public class EntityNotFoundException : ExceptionsBase
    {
        public EntityNotFoundException(string entity, Guid id)
            : base($"{entity} not found (ID: {id}).") { }
    }
}
