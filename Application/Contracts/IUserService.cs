using Application.Users.Models;


namespace Application.Contracts
{
    public interface IUserService
    {
        Task<LoginResult> LoginAsync(string email, string password);
        Task RegisterGuestAsync(RegisterHandler registerRequest);
    }
}
