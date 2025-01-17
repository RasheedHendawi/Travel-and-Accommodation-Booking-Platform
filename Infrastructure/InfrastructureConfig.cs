using Infrastructure.Authentication.JWT;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static  class InfraStructureConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddAuthService();

            return services;
        }
    }
}
