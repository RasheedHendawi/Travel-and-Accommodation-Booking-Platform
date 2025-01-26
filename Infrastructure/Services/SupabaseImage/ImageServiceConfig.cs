using Domain.Interfaces.Services;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;
using Shared.ValidatorHelper;

namespace Infrastructure.Services.SupabaseImage
{
    public static class ImageServiceConfig
    {
        public static IServiceCollection AddImageService(this IServiceCollection services)
        {
            services.AddScoped<IValidator<SupabaseConfig>, SupabaseValidator>();

            services.AddOptions<SupabaseConfig>()
                .BindConfiguration(nameof(SupabaseConfig))
                .FluentValidation()
                .ValidateOnStart();

            services.AddScoped<IImageService, SupabaseService>();

            return services;
        }
    }
}
