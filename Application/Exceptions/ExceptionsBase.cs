

namespace Application.Exceptions
{
    public abstract class ExceptionsBase(string message) : Exception(message)
    {
        public virtual string Header => "Exception";
    }
}
