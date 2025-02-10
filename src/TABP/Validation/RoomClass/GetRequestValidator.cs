using Application.DTOs.RoomClass;
using FluentValidation;
using static Domain.Rules.SharedRules;
namespace TABP.Validation.RoomClass
{
    public class GetRequestValidator : AbstractValidator<ResourcesQueryRequest>
    {
        public GetRequestValidator()
        {
            RuleFor(x => x.SearchTerm)
                .MaximumLength(TextMaxSize);
            RuleFor(x => x.SortOrder).IsInEnum();
            RuleFor(x => x.PageNumber).GreaterThan(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
