using Domain.Interfaces.Authentication;
using FluentValidation;
using Infrastructure.Authentication.JWT.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ValidatorHelper;
using System.Text;


namespace Infrastructure.Authentication.JWT
{
    public static class AuthServiceConfig
    {
        public static IServiceCollection AddAuthService
            (this IServiceCollection services)
        {
            services.AddScoped<IValidator<JwtSettings>, JwtSettingsValidator>();

            services.AddOptions<JwtSettings>()
                    .BindConfiguration(nameof(JwtSettings))
                    .FluentValidation()
                    .ValidateOnStart();

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
                .AddJwtBearer(options =>
            {
                using var scope = services.BuildServiceProvider().CreateScope();
                var con = scope.ServiceProvider.GetRequiredService<IOptions<JwtSettings>>().Value;

                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = con.Issuer,
                    ValidAudience = con.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(con.Key)),
                    ClockSkew = TimeSpan.Zero
                };
            });
            services.AddTransient<IJwtGenerator, JwtGenerator>();
            return services;
        }
    }
}
