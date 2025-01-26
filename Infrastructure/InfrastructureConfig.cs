using Infrastructure.Authentication.JWT;
using Infrastructure.Persistence;
using Infrastructure.Services.Email;
using Infrastructure.Services.PDF;
using Infrastructure.Services.SupabaseImage;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static  class InfrastructureConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddPersistence(config)
                .AddAuthService()
                .AddImageService()
                .AddPdf()
                .AddEmailService();
                

            return services;
        }
    }
}
