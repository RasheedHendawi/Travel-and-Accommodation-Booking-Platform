using Domain.Entities;
using Domain.Models;

namespace Domain.Interfaces.Authentication
{
    public interface IJwtGenerator
    {
        JwtToken GenerateToken(User user);
    }
}
