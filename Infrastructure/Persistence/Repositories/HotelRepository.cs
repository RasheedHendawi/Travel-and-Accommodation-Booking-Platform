using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Persistence.Repositories
{
    public class HotelRepository(HotelBookingPlatformDbContext context) : IHotelRepository
    {
        public async Task<Hotel> CreateAsync(Hotel hotel)
        {
            var tmpHotel = await context.Hotels.AddAsync(hotel);
            return tmpHotel.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            if(! await context.Hotels.AnyAsync(r => r.Id == id))
                throw new Exception("Hotel with this Id not Found");
            var hotel = context.ChangeTracker.Entries<Hotel>().FirstOrDefault(e => e.Entity.Id == id)?.Entity
                ?? new Hotel { Id = id };
            context.Hotels.Remove(hotel);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> expression)
        {
            return await context.Hotels.AnyAsync(expression);
        }

        public async Task<Hotel?> GetByIdAsync(Guid id, bool includeCity = false, bool includeThmbnail = false, bool includeGallary = false)
        {
            var query = context.Hotels
                .Where(h => h.Id == id);
            if(includeCity)
                query = query.Include(h => h.City);
            var hotel = await query.FirstOrDefaultAsync();
            if (hotel is null)
                return hotel;
            if(includeThmbnail)
                hotel.Thumbnail = await context.Images.FirstOrDefaultAsync(
                    i => i.EntityId == hotel.Id && i.Type == Domain.Enums.ImageType.Thumbnail);
            return hotel;
        }

        public async Task<PaginatedList<HotelForManagement>> GetForManagementAsync(Query<Hotel> query)
        {
            var queryable = context.Hotels
                .Where(query.Filter)
                .Sort(SortingExpressions.GetHotelSortExpression(query.SortColumn), query.SortOrder)
                .Select(h => new HotelForManagement
                {
                    Id = h.Id,
                    Name = h.Name,
                    StarRating = h.StartRating,
                    Owner = h.Owner,
                    NumberOfRooms = h.RoomClasses.SelectMany(rc => rc.Rooms).Count(),
                    CreatedAt = h.CreatedAt,
                    UpdatedAt = h.UpdatedAt
                });
            var tmpItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .ToListAsync();
            return new PaginatedList<HotelForManagement>(tmpItems,
                await queryable.GetPaginationDataAsync(query.PageNumber, query.PageSize));
        }

        public async Task<PaginatedList<HotelSearchResult>> GetForSearchAsync(Query<Hotel> query)
        {
            var queryable = context.Hotels
                  .Where(query.Filter)
                  .Select(h => new HotelSearchResult
                  {
                      Id = h.Id,
                      Name = h.Name,
                      StarRating = h.StartRating,
                      ReviewsRating = h.ReviewsRating,
                      BriefDescription = h.BriefDescription,
                      PricePerNightStartingAt = h.RoomClasses.Min(rc => rc.Price)
                  })
                  .Sort(SortingExpressions.GetHotelSearchSortExpression(query.SortColumn), query.SortOrder);

            var requestedPage = queryable.GetPage(
              query.PageNumber, query.PageSize);

            var itemsToReturn = (await requestedPage.Select(h => new
            {
                Hotel = h,
                Thumbnail = context.Images
                  .Where(i => i.EntityId == h.Id && i.Type == ImageType.Thumbnail)
                  .ToList()
            }).ToListAsync())
              .Select(h =>
              {
                  h.Hotel.Thumbnail = h.Thumbnail.FirstOrDefault();

                  return h.Hotel;
              });

            return new PaginatedList<HotelSearchResult>(
              itemsToReturn,
              await queryable.GetPaginationDataAsync(
                query.PageNumber,
                query.PageSize));
        }

        public async Task UpdateAsync(Hotel hotel)
        {
            if (!await context.Hotels.AnyAsync(h => h.Id == hotel.Id))
            {
                throw new Exception("Hotel with this Id not Found");
            }
            context.Hotels.Update(hotel);
        }

        public async Task UpdateReviewById(Guid id, double newRating)
        {
            if (!await context.Hotels.AnyAsync(h => h.Id == id))
            {
                throw new Exception("Hotel with this Id not Found");
            }
            var hotelEntity = context.ChangeTracker.Entries<Hotel>().FirstOrDefault(e => e.Entity.Id == id)?.Entity
                ?? new Hotel { Id = id };
            hotelEntity.ReviewsRating = newRating;
            context.Hotels.Update(hotelEntity);
        }
    }
}
