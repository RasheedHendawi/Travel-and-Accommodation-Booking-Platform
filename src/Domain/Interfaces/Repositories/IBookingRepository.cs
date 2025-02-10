
using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<PaginatedList<Booking>> GetAsync(Query<Booking> query);
        Task<Booking?> GetByIdAsync(Guid id);
        Task<Booking?> GetByIdAsync(Guid id, Guid guestId, bool includeInvoice = false);
        Task DeleteAsync(Guid id);
        Task<Booking> CreateAsync(Booking booking);
        Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate);
        Task<IEnumerable<Booking>> GetRecentBookingsByGuestId(Guid guestId, int count);
    }
}
