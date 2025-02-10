using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;


namespace Infrastructure.Persistence.Repositories
{
    public class RoomRepository(HotelBookingPlatformDbContext context) : IRoomRepository
    {
        public async Task<Room> CreateAsync(Room room)
        {
            ArgumentNullException.ThrowIfNull(room);

            var createdRoom = await context.Rooms.AddAsync(room);

            return createdRoom.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await context.Rooms.AnyAsync(r => r.Id == id))
            {
                throw new Exception("Room with this Id not Found");
            }

            var roomEntity = context.ChangeTracker.Entries<Room>()
                               .FirstOrDefault(e => e.Entity.Id == id)?.Entity
                             ?? new Room { Id = id };

            context.Rooms.Remove(roomEntity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Room, bool>> predicate)
        {
            return await context.Rooms.AnyAsync(predicate);
        }

        public async Task<PaginatedList<Room>> GetAsync(Query<Room> query)
        {
            var queryable = context.Rooms
              .Where(query.Filter)
              .Sort(r => r.Id, query.SortOrder);

            var itemsToReturn = await queryable
              .GetPage(query.PageNumber, query.PageSize)
              .AsNoTracking()
              .ToListAsync();

            return new PaginatedList<Room>(
              itemsToReturn,
              await queryable.GetPaginationDataAsync(
                query.PageNumber,
                query.PageSize));
        }

        public async Task<Room?> GetByIdAsync(Guid roomClassId, Guid id)
        {
            return await context.Rooms
              .FirstOrDefaultAsync(r => r.Id == id && r.RoomClassId == roomClassId);
        }

        public async Task<Room?> GetByIdWithRoomClassAsync(Guid roomId)
        {
            var currentDateTime = DateTime.UtcNow;

            return await context.Rooms
              .Include(r => r.RoomClass)
              .ThenInclude(rc => rc.Discounts.Where(d => d.StartDate <= currentDateTime && d.EndDate > currentDateTime))
              .FirstOrDefaultAsync(r => r.Id == roomId);
        }

        public async Task<PaginatedList<RoomForManagement>> GetForManagementAsync(Query<Room> query)
        {
            var currentDateUtc = DateOnly.FromDateTime(DateTime.UtcNow);

            var queryable = context.Rooms
              .Where(query.Filter)
              .Sort(SortingExpressions.GetRoomSortExpression(query.SortColumn), query.SortOrder)
              .Select(r => new RoomForManagement
              {
                  Id = r.Id,
                  RoomClassId = r.RoomClassId,
                  Number = r.Number,
                  IsAvailable = !r.Bookings.Any(
                  b => b.CheckIn >= currentDateUtc
                       && b.CheckOut <= currentDateUtc),
                  CreatedAt = r.CreatedAt,
                  UpdatedAt = r.UpdatedAt
              });

            var itemsToReturn = await queryable
              .GetPage(query.PageNumber, query.PageSize)
              .ToListAsync();

            return new PaginatedList<RoomForManagement>(
              itemsToReturn,
              await queryable.GetPaginationDataAsync(
                query.PageNumber,
                query.PageSize));
        }

        public async Task UpdateAsync(Room room)
        {
            ArgumentNullException.ThrowIfNull(room);

            if (!await context.Rooms.AnyAsync(r => r.Id == room.Id))
                throw new Exception("Room with this Id not Found");

            context.Rooms.Update(room);
        }
    }
}
