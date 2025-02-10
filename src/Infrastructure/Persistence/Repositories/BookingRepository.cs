using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class BookingRepository(HotelBookingPlatformDbContext context) : IBookingRepository
    {
        public async Task<Booking> CreateAsync(Booking booking)
        {
            ArgumentNullException.ThrowIfNull(booking);
            var tmpCreate = await context.Bookings.AddAsync(booking);
            return tmpCreate.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            
            var tmpId = await context.Bookings.AnyAsync(x => x.Id == id);
            if (!tmpId)
            {
                throw new Exception("Booking not found");
            }
            var tmpDelete =  context.ChangeTracker.Entries<Booking>()
                .FirstOrDefault(x => x.Entity.Id == id)?.Entity ?? new Booking { Id = id };
            context.Bookings.Remove(tmpDelete);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Booking, bool>> predicate)
        {
            var tmpPredicate = await context.Bookings.AnyAsync(predicate);
            return tmpPredicate;
        }

        public async Task<PaginatedList<Booking>> GetAsync(Query<Booking> query)
        {
            var tmpQuery = context.Bookings
                .Where(query.Filter).Sort(SortingExpressions.GetBookingSortExpression(query.SortColumn), query.SortOrder);
            var returnItem = await tmpQuery.GetPage(query.PageNumber, query.PageSize)
                .AsNoTracking()
                .Include(b => b.Hotel)
                .ToListAsync();
            return new PaginatedList<Booking>(
                returnItem,
                await tmpQuery.GetPaginationDataAsync(query.PageNumber, query.PageSize)
                );
        }

        public async Task<Booking?> GetByIdAsync(Guid id)
        {
            return await context.Bookings.FindAsync([id]);
        }

        public Task<Booking?> GetByIdAsync(Guid id, Guid guestId, bool includeInvoice = false)
        {
            var tmpBooking = context.Bookings.Where(u => u.Id == id && u.GuestId == guestId).Include(b => b.Hotel).ThenInclude(h => h.City);
            if (includeInvoice)
            {
                tmpBooking.Include(x => x.Invoice);
            }
            return tmpBooking.FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<Booking>> GetRecentBookingsByGuestId(Guid guestId, int count)
        {
            var tmpRecentBookings =
                from rb in (
                from b in
                    from rb in context.Bookings.ToLinqToDB()
                    where rb.GuestId == guestId
                    select new
                    {
                        Booking = rb,
                        Rank = LinqToDB.AnalyticFunctions.
                        RowNumber(LinqToDB.Sql.Ext).Over()
                        .PartitionBy(rb.HotelId)
                        .OrderByDesc(rb.BookingDate).ToValue()
                    }
                where b.Rank <= 1
                select b.Booking).Take(count)
                join h in context.Hotels.Include(h => h.City).ToLinqToDB
                () on rb.HotelId equals h.Id
                join j in context.Images.ToLinqToDB()
                on h.Id equals j.EntityId into images
                from img in images.DefaultIfEmpty()
                where img.Type == ImageType.Thumbnail
                select new { Booking = rb, Thumbnail = img };

            var tmpReturn = await tmpRecentBookings.AsNoTracking().ToListAsync();
            return tmpReturn.Select(br => { br.Booking.Hotel.Thumbnail = br.Thumbnail; return br.Booking; });
        }
    }
}
