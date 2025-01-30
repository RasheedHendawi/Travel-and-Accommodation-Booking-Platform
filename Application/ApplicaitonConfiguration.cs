using System.Reflection;
using Application.Cities;
using Application.Contracts;
using Application.Hotels;
using Application.Owners;
using Application.Reviews;
using Application.Users;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class ApplicaitonConfiguration
    {
        public static IServiceCollection AddApplication(this IServiceCollection services)
        {
            services.AddAutoMapper(Assembly.GetExecutingAssembly());
            RegisterServices(services);
            return services;
        }

        private static void RegisterServices(IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<ICityService, CityService>();
            services.AddScoped<IOwnerService, OwnerService>();
            services.AddScoped<IHotelService, HotelsService>();
            services.AddScoped<IReviewService, ReviewService>();
        }
    }
}
