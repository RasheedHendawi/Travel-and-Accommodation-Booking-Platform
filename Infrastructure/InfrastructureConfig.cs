using Domain.Interfaces.Authentication;
using Infrastructure.Authentication.JWT;
using Infrastructure.Authentication.JWT.Validator;
using Infrastructure.Persistence;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static  class InfrastructureConfig
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
        {
            services.AddPersistence(config)
                .AddAuthService();
                

            return services;
        }
    }
}
