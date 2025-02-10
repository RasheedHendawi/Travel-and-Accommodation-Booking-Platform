using Application.DTOs.Cities;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Cities
{
    public class GetRequestValidator : AbstractValidator<CitiesGetHandler>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.SearchTerm)
                .MaximumLength(TextGeneralSize);

            RuleFor(x => x.SortColumn)
                .Must(BeAValidSortColumn)
                .WithMessage("Should be a valid sort column");
        }
        private static bool BeAValidSortColumn(string? sortColumn)
        {
            if (string.IsNullOrEmpty(sortColumn))
            {
                return true;
            }

            var validColumns = new[]
            {
              "id",
              "Name",
              "Country",
              "PostOffice"
            };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
