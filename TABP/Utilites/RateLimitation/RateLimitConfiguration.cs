using FluentValidation;
using LinqToDB.Common;
using Microsoft.AspNetCore.RateLimiting;
using Microsoft.Extensions.Options;
using Shared.ValidatorHelper;

namespace TABP.Utilites.RateLimitation;

public static class RateLimitConfiguration
{
    public static IServiceCollection AddRateLimiting(this IServiceCollection services)
    {
        services.AddScoped<IValidator<RateLimitConfig>, RateLimitValidator>();

        services.AddOptions<RateLimitConfig>()
            .BindConfiguration("RateLimitConfig")
            .FluentValidation()
            .ValidateOnStart();

        services.AddRateLimiter(options =>
        {
            using var scope = services.BuildServiceProvider().CreateScope();

            var rateLimiterConfig = scope.ServiceProvider
                .GetRequiredService<IOptions<RateLimitConfig>>().Value;


            options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;

            options.AddFixedWindowLimiter("FixedWindow", limiterOptions =>
            {
                limiterOptions.PermitLimit = rateLimiterConfig.MaxRequests;
                limiterOptions.Window = TimeSpan.FromSeconds(rateLimiterConfig.WindowDurationInSeconds);
                limiterOptions.QueueProcessingOrder = rateLimiterConfig.RequestQueueOrder;
                limiterOptions.QueueLimit = rateLimiterConfig.MaxQueueSize;
            });
        });

        return services;
    }
}
