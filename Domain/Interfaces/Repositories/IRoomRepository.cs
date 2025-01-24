using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;


namespace Domain.Interfaces.Repositories
{
    public interface IRoomRepository
    {
        Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate);

        Task<PaginatedList<RoomForManagement>> GetForManagementAsync(
          Query<Room> query);

        Task<Room?> GetByIdAsync(Guid roomClassId, Guid id);

        Task<Room> CreateAsync(Room room);

        Task UpdateAsync(Room room);

        Task DeleteAsync(Guid id);

        Task<PaginatedList<Room>> GetAsync(Query<Room> query);

        Task<Room?> GetByIdWithRoomClassAsync(Guid roomId);
    }
}
