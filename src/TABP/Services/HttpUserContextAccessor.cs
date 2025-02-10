using Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;

namespace TABP.Services
{
    public class HttpUserContextAccessor(IHttpContextAccessor httpContextAccessor) : IHttpUserContextAccessor
    {
        private readonly ClaimsPrincipal? _user = httpContextAccessor.HttpContext?.User;

        public Guid Id => _user?.Identity?.IsAuthenticated == true
            ? Guid.Parse(_user.FindFirstValue(ClaimTypes.NameIdentifier) ?? throw new Exception("User ID not found"))
            : throw new Exception("Not Authenticated");

        public string Role => _user?.Identity?.IsAuthenticated == true
            ? _user.FindFirstValue(ClaimTypes.Role) ?? throw new Exception("Role not found")
            : throw new Exception("Not Authenticated");

        public string Email => _user?.Identity?.IsAuthenticated == true
            ? _user.FindFirstValue(ClaimTypes.Email) ?? throw new Exception("Email not found")
            : throw new Exception("Not Authenticated");
    }
}

