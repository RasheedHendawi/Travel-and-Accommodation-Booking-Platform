using Domain.Interfaces.Repositories;
using Domain.Entities;
using Infrastructure.Persistence.ContextDb;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class RoleRepository(HotelBookingPlatformDbContext context) : IRoleRepository
    {
        public async Task<Role?> GetByNameAsync(string name)
        {
            return await context.Roles.FirstOrDefaultAsync(x => x.Name == name);
        }
    }
}
