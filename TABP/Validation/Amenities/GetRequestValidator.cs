using Application.DTOs.Amenities;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Amenities
{
    public class GetRequestValidator : AbstractValidator<AmenitiesGetRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.SearchTerm)
              .MaximumLength(TextMaxSize);
            RuleFor(x => x.SortColumn)
                  .Must(BeAValidSortColumn)
                  .WithMessage("Must be a valid sort column");
        }
        private static bool BeAValidSortColumn(string? sortColumn)
        {
            if (string.IsNullOrEmpty(sortColumn))
            {
                return true;
            }

            var validColumns = new[]{"id","Name"};

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
