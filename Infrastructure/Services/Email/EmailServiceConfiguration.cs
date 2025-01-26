using Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.ValidatorHelper;

namespace Infrastructure.Services.Email
{
    public static class EmailServiceConfiguration
    {
        public static IServiceCollection AddEmailService(this IServiceCollection services)
        {
            services.AddScoped<IValidator<EmailConfig>, EmailConfigValidator>();

            services.AddOptions<EmailConfig>()
              .BindConfiguration(nameof(EmailConfig))
              .FluentValidation()
              .ValidateOnStart();

            services.AddTransient<IEmailService, EmailService>();

            return services;
        }
    }
}
