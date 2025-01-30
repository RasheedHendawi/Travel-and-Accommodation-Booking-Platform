using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TABP.Services
{
    public class HttpUserContextAccessor(IHttpContextAccessor httpContextAccessor) : IHttpUserContextAccessor
    {
        public Guid Id => Guid.Parse(
            httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier) ??
            throw new Exception("Not Authenticated"));

        public string Role => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Role) ??
                              throw new Exception("Not Authenticated");

        public string Email => httpContextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.Email) ??
                               throw new Exception("Not Authenticated");
    }
}
