using Domain.Interfaces.Authentication;
using FluentValidation;
using Infrastructure.Authentication.JWT.Validator;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Shared.ValidatorHelper;
using System.Text;


namespace Infrastructure.Authentication.JWT
{
    public static class AuthServiceConfig
    {
        public static IServiceCollection AddAuthServiceConfig
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
            }).AddJwtBearer(options =>
            {
                var jwtSettings = services.BuildServiceProvider().GetService<IOptions<JwtSettings>>()!.Value;
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidIssuer = jwtSettings.Issuer,
                    ValidateAudience = true,
                    ValidAudience = jwtSettings.Audience,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Key)),
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };
            });

            services.AddTransient<IJwtGenerator, JwtGenerator>();
            return services;
        }
    }
}
