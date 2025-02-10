using Application.DTOs.Hotels;
using FluentValidation;
using static Domain.Rules.RoomClassRules;
using static Domain.Rules.SharedRules;
using static Domain.Rules.HotelRules;
namespace TABP.Validation.Hotels
{
    public class SearchRequestValidator : AbstractValidator<HotelSearchRequest>
    {
        public SearchRequestValidator()
        {
            RuleFor(x => x.SearchTerm)
                .MaximumLength(TextMaxSize);
            RuleFor(x => x.SortOrder)
                .IsInEnum();
            RuleFor(x => x.SortColumn)
                .MaximumLength(TextMaxSize)
                .Must(BeAValidSortColumn)
                .WithMessage("Search sort column not valid");
            RuleFor(x => x.PageNumber)
                .GreaterThan(0);
            RuleFor(x => x.PageSize)
                .GreaterThan(0);
            RuleFor(x => x.CheckInDate)
                .NotEmpty()
                .GreaterThanOrEqualTo(DateOnly.FromDateTime(DateTime.UtcNow));
            RuleFor(x => x.CheckOutDate)
                .NotEmpty();
            RuleFor(x => x.NumberOfAdults)
                .GreaterThanOrEqualTo(MinAdults)
                .LessThanOrEqualTo(MaxAdults);
            RuleFor(x => x.NumberOfChildren)
                .GreaterThanOrEqualTo(MinChildren)
                .LessThanOrEqualTo(MaxChildren);
            RuleFor(x => x.NumberOfRooms)
                .GreaterThan(0);
            RuleFor(x => x.MinPrice)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.MaxPrice)
                .GreaterThanOrEqualTo(0);
            RuleFor(x => x.MinStarRating)
                .InclusiveBetween(MinStars, MaxStars);
            RuleFor(x => x.RoomTypes)
                .NotEmpty();
            RuleFor(x => x.Amenities)
                .NotEmpty();
            When(x => x.MinStarRating is not null, () =>
            {
                RuleFor(x => x.MinStarRating)
                  .InclusiveBetween(MinStars, MaxStars);
            });
            When(x => x.RoomTypes is not null, () =>
            {
                RuleForEach(x => x.RoomTypes)
                  .IsInEnum()
                  .WithMessage("Room type not valid");
            });

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
              "name",
              "starRating",
              "price",
              "reviewsRating"
            };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
