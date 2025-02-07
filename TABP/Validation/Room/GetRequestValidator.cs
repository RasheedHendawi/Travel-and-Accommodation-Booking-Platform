using Application.DTOs.Rooms;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.Room
{
    public class GetRequestValidator : AbstractValidator<RoomsGetRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.SearchTerm)
                .MaximumLength(TextMaxSize);
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
              "number"
            };

            return Array.Exists(validColumns,
              col => string.Equals(col, sortColumn, StringComparison.OrdinalIgnoreCase));
        }
    }
}
