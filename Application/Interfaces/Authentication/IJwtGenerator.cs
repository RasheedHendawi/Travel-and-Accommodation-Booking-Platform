using Domain.Entities;
using Domain.Models;

namespace Application.Interfaces.Authentication
{
    public interface IJwtGenerator
    {
        JwtToken GenerateToken(User user);
    }
}
