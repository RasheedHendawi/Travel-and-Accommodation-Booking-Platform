using System.Reflection;
using Application.Contracts;
using Application.Services.Amenities;
using Application.Services.Bookings;
using Application.Services.Cities;
using Application.Services.Discounts;
using Application.Services.Hotels;
using Application.Services.Owners;
using Application.Services.Reviews;
using Application.Services.RoomClasses;
using Application.Services.Rooms;
using Application.Services.Users;
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
            services.AddScoped<IAmenityService, AmenityService>();
            services.AddScoped<IRoomClassService, RoomClassService>();
            services.AddScoped<IDiscountService, DiscountService>();
            services.AddScoped<IRoomService, RoomService>();
            services.AddScoped<IBookingsService, BookingService>();
        }
    }
}
