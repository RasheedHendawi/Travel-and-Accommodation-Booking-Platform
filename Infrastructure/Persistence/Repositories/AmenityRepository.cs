using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Persistence.Repositories
{
    public class AmenityRepository(HotelBookingPlatformDbContext context) : IAmenityRepository
    {
        public async Task<Amenity> CreateAsync(Amenity amenity)
        {
            var tmpAmentiy = await context.Amenities.AddAsync(amenity);
            return tmpAmentiy.Entity;
        }

        public Task<bool> ExistAsync(Expression<Func<Amenity, bool>> predicate)
        {
            return context.Amenities.AnyAsync(predicate);
        }

        public async Task<PaginatedList<Amenity>> GetAsync(Query<Amenity> query)
        {
            var tmpQuery = context.Amenities
              .Where(query.Filter).Sort(SortingExpressions.GetAmenitySortExpression(query.SortColumn), query.SortOrder);
            var toReturn = await tmpQuery.GetPage(query.PageNumber, query.PageSize).AsNoTracking()
                .ToListAsync();
            return new PaginatedList<Amenity>(toReturn, await tmpQuery.GetPaginationDataAsync(query.PageNumber, query.PageSize));
        }

        public async Task<Amenity?> GetByIdAsync(Guid id)
        {
            return await context.Amenities.FindAsync([id]);
        }

        public async Task UpdateAsync(Amenity amenity)
        {
            ArgumentNullException.ThrowIfNull(amenity);
            if (!await context.Amenities.AnyAsync(a => a.Id == amenity.Id))
                throw new Exception("Id not Found!");
            context.Amenities.Update(amenity);
        }
    }
}
