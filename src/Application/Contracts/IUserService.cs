using Application.DTOs.Users;


namespace Application.Contracts
{
    public interface IUserService
    {
        Task<LoginResponse> LoginAsync(string email, string password);
        Task RegisterGuestAsync(RegisterRequest registerRequest);
    }
}
