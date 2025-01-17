using Infrastructure.Persistence.DbContext;
using LinqToDB.EntityFrameworkCore;
using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Persistence
{
    public static class PersistenceConfig
    {
        public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext(configuration)
                .AddHashing();
            return services;
        }
        private static IServiceCollection AddDbContext(
          this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HotelBookingPlatformDbContext>(options =>
            {
                LinqToDBForEFTools.Initialize();

                options.UseSqlServer(configuration.GetConnectionString("ConnectionToSql"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure(10))
                  .UseLinqToDB();
            });

            return services;
        }
        private static IServiceCollection AddHashing(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher, PasswordHasher>();

            return services;
        }
    }
}
