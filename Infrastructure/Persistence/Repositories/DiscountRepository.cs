using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using Infrastructure.Utilites;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure.Persistence.Repositories
{
    public class DiscountRepository(HotelBookingPlatformDbContext context) : IDiscountRepository
    {
        public async Task<Discount> CreateAsync(Discount discount)
        {
            ArgumentNullException.ThrowIfNull(discount, nameof(discount));
            var tmpDiscount = await context.Discounts.AddAsync(discount);
            return tmpDiscount.Entity;
        }

        public async Task DeleteAsync(Guid id)
        {
            if (!await context.Discounts.AnyAsync(r => r.Id == id))
            {
                throw new Exception("Discount with this Id not Found");
            }
            var entity = context.ChangeTracker.Entries<Discount>().FirstOrDefault(e => e.Entity.Id == id)?.Entity
                ?? new Discount { Id = id };
            context.Discounts.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Discount, bool>> expression)
        {
            return await context.Discounts.AnyAsync(expression);
        }

        public async Task<PaginatedList<Discount>> GetAsync(Query<Discount> query)
        {
            var queryable = context.Discounts
                .Where(query.Filter)
                .Sort(SortingExpressions.GetDiscountSortExpression(query.SortColumn),
                query.SortOrder);
            var tmpItems = await queryable
                .GetPage(query.PageNumber, query.PageSize)
                .AsNoTracking()
                .ToListAsync();
            return new PaginatedList<Discount>(tmpItems, 
                await queryable.GetPaginationDataAsync(query.PageNumber, query.PageSize));
        }

        public async Task<Discount?> GetByIdAsync(Guid roomClassID, Guid id)
        {
            return await  context.Discounts
                .FirstOrDefaultAsync(d => d.RoomClassId == roomClassID && d.Id == id);
        }
    }
}
