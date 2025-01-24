using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Persistence.ContextDb;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.Repositories
{
    public class UserRepository(HotelBookingPlatformDbContext context, IPasswordHasher<User> passwordHasher) : IUserRepository
    {
        private readonly IPasswordHasher<User> _passwordHasher = passwordHasher;
        private readonly HotelBookingPlatformDbContext _context = context;
        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            var user = await _context.Users
                .Include(x => x.Roles)
                .FirstOrDefaultAsync(x => x.Email == email);
            if (user is null)
            {
                return null;
            }
            var passwordHash = _passwordHasher.VerifyHashedPassword(user, user.Password, password);
            return passwordHash == PasswordVerificationResult.Success ? user : null;
        }

        public async Task CreateAsync(User user)
        {
            ArgumentNullException.ThrowIfNull(user);

            user.Password = _passwordHasher.HashPassword(user, user.Password);

            await _context.Users.AddAsync(user);
        }

        public async Task<bool> ExistsByEmailAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> ExistsByIdAsync(Guid id)
        {
            return await _context.Users
              .AnyAsync(u => u.Id == id);
        }

        public async Task<User?> GetByIdAsync(Guid id)
        {
            return await _context.Users
              .FindAsync([id]);
        }
    }
}
