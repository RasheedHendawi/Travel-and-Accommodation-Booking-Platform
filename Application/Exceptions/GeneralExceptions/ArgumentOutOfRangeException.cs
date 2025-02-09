

namespace Application.Exceptions.GeneralExceptions
{
    public class ArgumentOutOfRangeException : ExceptionsBase
    {
        public ArgumentOutOfRangeException(string parameter)
            : base($"Argument '{parameter}' is out of range.") { }
    }
}
