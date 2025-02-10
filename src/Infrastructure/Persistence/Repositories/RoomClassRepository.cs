using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Threading;

namespace Infrastructure.Persistence.Repositories
{
    public class RoomClassRepository(HotelBookingPlatformDbContext context) : IRoomClassRepository
    {
        public async Task<RoomClass> CreateAsync(RoomClass roomClass)
        {
            ArgumentNullException.ThrowIfNull(roomClass);

            var createdRoomClass = await context.RoomClasses.AddAsync(roomClass);

            return createdRoomClass.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await context.RoomClasses.AnyAsync(r => r.Id == id))
            {
                throw new Exception("RoomClass with this Id not Found");
            }

            var entity = context.ChangeTracker.Entries<RoomClass>()
                           .FirstOrDefault(e => e.Entity.Id == id)?.Entity
                         ?? new RoomClass { Id = id };

            context.RoomClasses.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<RoomClass, bool>> predicate)
        {
            return await context.RoomClasses.AnyAsync(predicate);
        }

        public async Task<PaginatedList<RoomClass>> GetAsync(Query<RoomClass> query, bool includeGallery = false)
        {
            var currentDateTime = DateTime.UtcNow;

            var queryable = context.RoomClasses
              .Include(rc => rc.Discounts
                .Where(d => currentDateTime >= d.StartDate && currentDateTime < d.EndDate))
              .Include(rc => rc.Amenities)
              .AsSplitQuery()
              .Where(query.Filter)
              .Sort(SortingExpressions.GetRoomClassSortExpression(query.SortColumn), query.SortOrder);

            var requestedPage = queryable.GetPage(query.PageNumber, query.PageSize);

            IEnumerable<RoomClass> itemsToReturn;

            if (includeGallery)
            {
                itemsToReturn = (await requestedPage.Select(rc => new
                {
                    RoomClass = rc,
                    Gallery = context.Images
                    .Where(i => i.EntityId == rc.Id && i.Type == ImageType.Gallery)
                    .ToList()
                }).AsNoTracking().ToListAsync())
                .Select(rc =>
                {
                    rc.RoomClass.Gallary = rc.Gallery;

                    return rc.RoomClass;
                });
            }
            else
            {
                itemsToReturn = await requestedPage
                  .AsNoTracking()
                  .ToListAsync();
            }

            return new PaginatedList<RoomClass>(
              itemsToReturn,
              await queryable.GetPaginationDataAsync(
                query.PageNumber,
                query.PageSize));
        }

        public async Task<RoomClass?> GetByIdAsync(Guid id)
        {
            return await context.RoomClasses
                .Include(rc => rc.Amenities)
                .Include(rc => rc.Discounts)
              .FirstOrDefaultAsync(rc => rc.Id == id);
        }

        public async Task<IEnumerable<RoomClass>> GetFeaturedDealsAsync(int count)
        {
            var currentDateTime = DateTime.UtcNow;

            var featuredDeals = await (from rcd in (from rc in (
                    from rc in context.RoomClasses.ToLinqToDB()
                    join d in context.Discounts
                        .Where(d => d.StartDate <= currentDateTime && d.EndDate > currentDateTime)
                        .ToLinqToDB()
                      on rc.Id equals d.RoomClassId
                    select new
                    {
                        RoomClass = rc,
                        ActiveDiscount = d,
                        Rank = LinqToDB.AnalyticFunctions.RowNumber(LinqToDB.Sql.Ext)
                        .Over()
                        .PartitionBy(rc.HotelId)
                        .OrderByDesc(d.Percentage)
                        .ThenBy(rc.Price)
                        .ToValue()
                    })
                        where rc.Rank <= 1
                        select new { rc.RoomClass, rc.ActiveDiscount }).Take(count)
                        join h in context.Hotels.Include(h => h.City).ToLinqToDB()
                           on rcd.RoomClass.HotelId equals h.Id
                        join i in context.Images.ToLinqToDB()
                           on h.Id equals i.EntityId into images
                        from img in images.DefaultIfEmpty()
                        where img.Type == ImageType.Thumbnail
                        select new { rcd.RoomClass, rcd.ActiveDiscount, Hotel = h, Thumbnail = img })
              .ToListAsync();

            return featuredDeals.Select(deal =>
            {
                deal.Hotel.Thumbnail = deal.Thumbnail;

                deal.RoomClass.Discounts.Add(deal.ActiveDiscount);

                deal.RoomClass.Hotel = deal.Hotel;

                return deal.RoomClass;
            });
        }

        public async Task UpdateAsync(RoomClass roomClass)
        {
            ArgumentNullException.ThrowIfNull(roomClass);

            if (!await ExistsAsync(rc => rc.Id == roomClass.Id))
                throw new Exception("RoomClass with this Id not Found");

            context.RoomClasses.Update(roomClass);
        }
    }
}
