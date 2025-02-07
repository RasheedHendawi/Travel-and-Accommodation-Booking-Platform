using Application.DTOs.Discounts;
using FluentValidation;

namespace TABP.Validation.Discount
{
    public class GetRequsetValidator : AbstractValidator<DiscountsGetRequest>
    {
        public GetRequsetValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
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
                  "creationDate",
                  "startDate",
                  "endDate"
                };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
