using Application.DTOs.Hotels;
using FluentValidation;

namespace TABP.Validation.Review
{
    public class GetRequestValidator : AbstractValidator<ReviewsGetRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.SortColumn)
                .Must(BeAValidSortColumn);
            RuleFor(x => x.SortOrder)
                .IsInEnum();
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);
            RuleFor(x => x.PageSize)
                .GreaterThan(0);
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
              "rating"
            };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
