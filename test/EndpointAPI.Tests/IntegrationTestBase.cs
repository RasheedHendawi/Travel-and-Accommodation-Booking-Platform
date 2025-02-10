///TODO : Uncomment the code below and then run the tests after fixing it
/*using Application.Contracts;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace EndpointAPI.Tests
{
    public abstract class IntegrationTestBase : IClassFixture<WebApplicationFactory<Program>>
    {
        protected readonly HttpClient Client;
        protected readonly Mock<IOwnerService> OwnerServiceMock = new();
        protected readonly Mock<IAmenityService> AmenityServiceMock = new();
        protected readonly Mock<IUserService> UserServiceMock = new();
        protected readonly Mock<IBookingsService> BookingsServiceMock = new();
        protected readonly Mock<ICityService> CityServiceMock = new();
        protected readonly Mock<IDiscountService> DiscountServiceMock = new();
        protected readonly Mock<IHotelService> HotelServiceMock = new();
        protected readonly Mock<IReviewService> ReviewServiceMock = new();
        protected readonly Mock<IRoomClassService> RoomClassServiceMock = new();
        protected readonly Mock<IRoomService> RoomServiceMock = new();

        public IntegrationTestBase(WebApplicationFactory<Program> factory)
        {
            Client = factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    services.AddScoped(_ => OwnerServiceMock.Object);
                    services.AddScoped(_ => AmenityServiceMock.Object);
                    services.AddScoped(_ => UserServiceMock.Object);
                    services.AddScoped(_ => BookingsServiceMock.Object);
                    services.AddScoped(_ => CityServiceMock.Object);
                    services.AddScoped(_ => DiscountServiceMock.Object);
                    services.AddScoped(_ => HotelServiceMock.Object);
                    services.AddScoped(_ => ReviewServiceMock.Object);
                    services.AddScoped(_ => RoomClassServiceMock.Object);
                    services.AddScoped(_ => RoomServiceMock.Object);
                });
            }).CreateClient();
        }
    }
}
*/