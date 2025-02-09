///TODO : Uncomment the code below and then run the tests after fixing it
/*using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using EndpointAPI.Tests.Helpers;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;


namespace EndpointAPI.Tests.Controllers
{
    public class HotelsControllerTests : IntegrationTestBase
    {
        public HotelsControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

        [Fact]
        public async Task GetHotelsForManagement_ShouldReturnOk_WhenUserIsAdmin()
        {
            var token = TestAuthHelper.GenerateTestToken("admin-user-id", "Admin");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await Client.GetAsync("/api/hotels");

            response.Should().HaveStatusCode(System.Net.HttpStatusCode.OK);
        }


        [Fact]
        public async Task GetHotelsForManagement_ShouldReturnForbidden_WhenUserIsNotAdmin()
        {
            var token = TestAuthHelper.GenerateTestToken("regular-user-id", "User");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

            var response = await Client.GetAsync("/api/hotels");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Forbidden);
        }

        [Fact]
        public async Task SearchAndFilterHotels_ShouldReturnOk_WhenValidRequest()
        {
            var response = await Client.GetAsync("/api/hotels/search");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.OK);
        }

        [Fact]
        public async Task GetHotel_ShouldReturnNotFound_WhenHotelDoesNotExist()
        {
            var response = await Client.GetAsync($"/api/hotels/{Guid.NewGuid()}");

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.NotFound);
        }

        [Fact]
        public async Task CreateHotel_ShouldReturnCreated_WhenValidRequest()
        {
            var token = TestAuthHelper.GenerateTestToken("admin-user-id", "Admin");
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
            var hotelRequest = new { Name = "Test Hotel", Location = "Test Location" };
            var content = new StringContent(JsonSerializer.Serialize(hotelRequest), Encoding.UTF8, "application/json");

            var response = await Client.PostAsync("/api/hotels", content);

            response.StatusCode.Should().Be(System.Net.HttpStatusCode.Created);
        }
    }
}
*/