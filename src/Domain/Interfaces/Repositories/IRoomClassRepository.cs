using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;


namespace Domain.Interfaces.Repositories
{
    public interface IRoomClassRepository
    {
        Task<bool> ExistsAsync(Expression<Func<RoomClass, bool>> predicate);
        Task<RoomClass?> GetByIdAsync(Guid id);

        Task<RoomClass> CreateAsync(RoomClass roomClass);

        Task UpdateAsync(RoomClass roomClass);

        Task DeleteAsync(Guid id);

        Task<PaginatedList<RoomClass>> GetAsync(Query<RoomClass> query,bool includeGallery = false);

        Task<IEnumerable<RoomClass>> GetFeaturedDealsAsync(int count);
    }
}
