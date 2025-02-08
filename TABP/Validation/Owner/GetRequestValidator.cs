using Application.DTOs.Owners;
using FluentValidation;

namespace TABP.Validation.Owner
{
    public class GetRequestValidator : AbstractValidator<OwnersGetRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.SortColumn)
                .Must(BeAValidSortColumn)
                .WithMessage("Sort column not valid");
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
              "FirstName",
              "LastName"
            };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
