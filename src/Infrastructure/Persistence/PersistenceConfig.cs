using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Persistence.Repositories;
using LinqToDB.EntityFrameworkCore;
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
                .AddHashing()
                .AddRepositories();
            return services;
        }
        private static IServiceCollection AddDbContext(
          this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<HotelBookingPlatformDbContext>(options =>
            {
                LinqToDBForEFTools.Initialize();

                options.UseSqlServer(configuration.GetConnectionString("ConnectionToSql"),
                    optionsBuilder => optionsBuilder.EnableRetryOnFailure(12))
                  .UseLinqToDB();
            });
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            return services;
        }

        private static IServiceCollection AddHashing(this IServiceCollection services)
        {
            services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
            return services;
        }
        private static IServiceCollection AddRepositories(this IServiceCollection services)
        {
            services.AddScoped<IAmenityRepository, AmenityRepository>()
              .AddScoped<IBookingRepository, BookingRepository>()
              .AddScoped<ICityRepository, CityRepository>()
              .AddScoped<IDiscountRepository, DiscountRepository>()
              .AddScoped<IHotelRepository, HotelRepository>()
              .AddScoped<IImageRepository, ImageRepository>()
              .AddScoped<IOwnerRepository, OwnerRepository>()
              .AddScoped<IRoleRepository, RoleRepository>()
              .AddScoped<IRoomClassRepository, RoomClassRepository>()
              .AddScoped<IRoomRepository, RoomRepository>()
              .AddScoped<IUserRepository, UserRepository>()
              .AddScoped<IReviewRepository, ReviewRepository>();

            return services;
        }

    }
}
