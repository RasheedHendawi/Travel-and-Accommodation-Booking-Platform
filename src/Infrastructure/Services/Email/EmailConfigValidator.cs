﻿using FluentValidation;

namespace Infrastructure.Services.Email
{
    public class EmailConfigValidator : AbstractValidator<EmailConfig>
    {
        public EmailConfigValidator()
        {
            RuleFor(x => x.Server)
              .NotEmpty();

            RuleFor(x => x.Port)
              .GreaterThan(0)
              .NotEmpty();

            RuleFor(x => x.Username)
              .NotEmpty();

            RuleFor(x => x.Password)
              .NotEmpty();

            RuleFor(x => x.FromEmail)
              .NotEmpty()
              .EmailAddress();
        }
    }
}
