using Application.DTOs.Rooms;
using FluentValidation;

namespace TABP.Validation.Room
{
    public class CreateRequestValidator : AbstractValidator<RoomCreationRequest>
    {
        public CreateRequestValidator()
        {
            RuleFor(x => x.Number)
                .NotEmpty();
        }
    }
}
