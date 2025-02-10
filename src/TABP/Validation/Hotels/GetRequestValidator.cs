using Application.DTOs.Hotels;
using FluentValidation;

namespace TABP.Validation.Hotels
{
    public class GetRequestValidator : AbstractValidator<HotelGetRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
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
              "Name"
            };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
