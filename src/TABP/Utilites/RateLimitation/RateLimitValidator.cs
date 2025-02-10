using FluentValidation;

namespace TABP.Utilites.RateLimitation
{
    public class RateLimitValidator : AbstractValidator<RateLimitConfig>
    {
        public RateLimitValidator()
        {
            RuleFor(x => x.MaxRequests)
              .NotEmpty()
              .GreaterThan(0);

            RuleFor(x => x.WindowDurationInSeconds)
              .NotEmpty()
              .GreaterThan(0);

            RuleFor(x => x.RequestQueueOrder)
              .NotNull()
              .IsInEnum();

            RuleFor(x => x.MaxQueueSize)
              .NotEmpty()
              .GreaterThan(0);
        }
    }
}
