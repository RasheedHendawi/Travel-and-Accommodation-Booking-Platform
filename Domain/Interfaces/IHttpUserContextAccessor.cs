

namespace Domain.Interfaces
{
    public interface IHttpUserContextAccessor
    {
        Guid Id { get; }

        string Role { get; }

        string Email { get; }
    }
}
