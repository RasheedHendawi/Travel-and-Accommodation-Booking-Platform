
namespace Shared.ValidatorHelper
{
    public static class ValidationMessages
    {
        public static string FailureMessages(string optionsType, string propertyName, string errorMessages)
        {
            return $"Fluent validation failed for '{optionsType}.{propertyName}' with the error: '{errorMessages}'.";
        }
    }
}
