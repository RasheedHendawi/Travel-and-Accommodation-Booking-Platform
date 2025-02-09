using EndpointAPI.Tests.Helpers;
using Infrastructure.Persistence.ContextDb;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.TestPlatform.TestHost;
namespace EndpointAPI.Tests
{
    public class CustomWebApplicationFactory : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                var descriptor = services.SingleOrDefault(d =>
                d.ServiceType == typeof(DbContextOptions<HotelBookingPlatformDbContext>));
                if (descriptor != null)
                {
                    services.Remove(descriptor);
                }

                services.AddDbContext<HotelBookingPlatformDbContext>(options =>
                {
                    options.UseInMemoryDatabase("TestingDb");
                });

                services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                    .AddJwtBearer(options =>
                    {
                        options.TokenValidationParameters = new TokenValidationParameters
                        {
                            ValidateIssuerSigningKey = true,
                            IssuerSigningKey = new SymmetricSecurityKey("ThisIsA32ByteLongSecretKeyForHS256!"u8.ToArray()),
                            ValidateIssuer = false,
                            ValidateAudience = false
                        };
                    });

                services.AddAuthorization(options =>
                {
                    options.AddPolicy("TestingPolicy", policy => policy.RequireAuthenticatedUser());
                });

                var serviceProvider = services.BuildServiceProvider();



                using (var scope = serviceProvider.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<HotelBookingPlatformDbContext>();
                    db.Database.EnsureCreated();
                    SeedTestData(db);
                }
            });
        }

        private void SeedTestData(HotelBookingPlatformDbContext context)
        {
            ///TODO: Add seed data
            //context.Users.Add(new User { Id = new Guid(), Role = "Admin" });
            context.SaveChanges();
        }
    }
}
