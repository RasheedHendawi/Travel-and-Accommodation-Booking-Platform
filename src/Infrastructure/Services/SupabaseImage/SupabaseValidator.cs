using FluentValidation;

namespace Infrastructure.Services.SupabaseImage
{
    public class SupabaseValidator : AbstractValidator<SupabaseConfig>
    {
        public SupabaseValidator()
        {
            RuleFor(x => x.Url)
                .NotEmpty()
                .WithMessage("Supabase URL cannot be empty.");

            RuleFor(x => x.Key)
                .NotEmpty()
                .WithMessage("Supabase Key cannot be empty.");

            RuleFor(x => x.Bucket)
                .NotEmpty()
                .WithMessage("Supabase Bucket cannot be empty.");
        }
    }
}
