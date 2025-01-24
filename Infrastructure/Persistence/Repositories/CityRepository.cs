using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using LinqToDB.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class CityRepository(HotelBookingPlatformDbContext context): ICityRepository
    {
        public async Task<City> CreateAsync(City city)
        {
            ArgumentNullException.ThrowIfNull(city, nameof(city));
            var tmpCity = await context.Cities.AddAsync(city);
            return tmpCity.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await context.Cities.AnyAsync(c => c.Id == id))
            {
                throw new Exception("City with this Id not Found");
            }
            var entity = context.ChangeTracker.Entries<City>().FirstOrDefault(e => e.Entity.Id == id)?
                .Entity ?? new City { Id = id };
            context.Cities.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<City, bool>> expression)
        {
            var tmpCity = await context.Cities.AnyAsync(expression);
            return tmpCity;
        }

        public async Task<City?> GetByIdAsync(Guid id)
        {
            return await context.Cities.FindAsync([id]);
        }

        public async Task<PaginatedList<CityForManagement>> GetForManagement(Query<City> query)
        {
            var queryable = context.Cities
                .Where(query.Filter)
                .Sort(SortingExpressions.GetCitySortExpression(query.SortColumn), query.SortOrder)
                .Select(c => new CityForManagement
                {
                    Id = c.Id,
                    Name = c.Name,
                    Country = c.Country,
                    PostOffice = c.PostOffice,
                    NumberOfHotels = c.Hotels.Count,
                    CreatedAt = c.CreatedAt,
                    UpdatedAt = c.UpdatedAt
                });
            var tmpItems = await queryable.GetPage(query.PageNumber, query.PageSize).ToListAsync();
            return new PaginatedList<CityForManagement>(tmpItems,
                await queryable.GetPaginationDataAsync(
                    query.PageNumber, query.PageSize));
        }

        public async Task<IEnumerable<City>> GetMostVisitedAsync(int count)
        {
            var mostVisitedCte = LinqToDB.LinqExtensions
                .AsCte(context.Bookings.GroupBy(b => b.Hotel.CityId))
                .OrderByDescending(g => g.Count())
                .Take(count).Select(g => g.Key);
            var query =
                from c in context.Cities.ToLinqToDB()
                join cityId in mostVisitedCte on c.Id equals cityId
                join j in context.Images.ToLinqToDB()
                on c.Id equals j.EntityId into images
                from img in images.DefaultIfEmpty()
                where img.Type == ImageType.Thumbnail
                select new { City = c, Thumbnail = img };
            var MostVisitedCities = await query.AsNoTracking().ToListAsync();
            return MostVisitedCities.Select(c =>
            {
                c.City.Thumbnail = c.Thumbnail;
                return c.City;
            });

        }

        public async Task UpdateAsync(City city)
        {
            ArgumentNullException.ThrowIfNull(city, nameof(city));
            if(! await context.Cities.AnyAsync(c => c.Id == city.Id))
            {
                throw new Exception("City with this Id not Found");
            }
            context.Cities.Update(city);
        }
    }
}
