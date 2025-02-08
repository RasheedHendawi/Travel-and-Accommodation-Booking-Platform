using Application.DTOs.Rooms;
using FluentValidation;

namespace TABP.Validation.Room
{
    public class UpdateRequestValidator : AbstractValidator<RoomUpdateRequest>
    {
        public UpdateRequestValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty();
        }
    }
}
