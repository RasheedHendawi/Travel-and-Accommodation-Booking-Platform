using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;


namespace Infrastructure.Persistence.Repositories
{
    internal class OwnerRepository(HotelBookingPlatformDbContext context) : IOwnerRepository
    {
        public async Task<Owner> CreateAsync(Owner owner)
        {
            ArgumentNullException.ThrowIfNull(owner);

            var tmpEntity = await context.AddAsync(owner);

            return tmpEntity.Entity;
        }

        public async Task<bool> ExistsAsync(Expression<Func<Owner, bool>> predicate)
        {
            return await context.Owners.AnyAsync(predicate);
        }

        public async Task<PaginatedList<Owner>> GetAsync(Query<Owner> query)
        {
            var queryable = context.Owners
              .Where(query.Filter)
              .Sort(SortingExpressions.GetOwnerSortExpression(query.SortColumn), query.SortOrder);

            var tmpItems = await queryable
              .GetPage(query.PageNumber, query.PageSize)
              .AsNoTracking()
              .ToListAsync();

            return new PaginatedList<Owner>(
              tmpItems,
              await queryable.GetPaginationDataAsync(
                query.PageNumber,
                query.PageSize));
        }

        public async Task<Owner?> GetByIdAsync(Guid id)
        {
            return await context.Owners
              .FindAsync([id]);
        }

        public async Task UpdateAsync(Owner owner)
        {
            ArgumentNullException.ThrowIfNull(owner);

            if (!await context.Owners.AnyAsync(
                  o => o.Id == owner.Id))
                throw new Exception("Owner with this Id not Found");

            context.Owners.Update(owner);
        }
    }
}
