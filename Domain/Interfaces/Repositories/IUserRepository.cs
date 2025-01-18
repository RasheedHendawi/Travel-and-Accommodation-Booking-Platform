using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User?> AuthenticateAsync(string email, string password);

        Task CreateAsync(User user);

        Task<bool> ExistsByEmailAsync(string email);
        Task<bool> ExistsByIdAsync(Guid id);
        Task<User?> GetByIdAsync(Guid id);
    }
}
