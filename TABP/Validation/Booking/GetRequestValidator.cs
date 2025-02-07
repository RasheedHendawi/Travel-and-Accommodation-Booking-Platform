using Application.DTOs.Bookings;
using FluentValidation;

namespace TABP.Validation.Booking
{
    public class GetRequestValidator : AbstractValidator<BookingsGetRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.PageNumber)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.PageSize)
                .GreaterThanOrEqualTo(1);
            RuleFor(x => x.SortColumn)
              .Must(BeAValidSortColumn)
              .WithMessage("SortColumn must be valid");
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
                  "bookingDate",
                  "checkIn",
                  "checkOut"
                };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
