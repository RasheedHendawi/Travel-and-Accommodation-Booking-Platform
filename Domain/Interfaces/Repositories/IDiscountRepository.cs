using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IDiscountRepository
    {
        Task DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<Discount, bool>> expression);
        Task<Discount?> GetByIdAsync(Guid roomClassID, Guid id);
        Task<Discount> CreateAsync(Discount discount);
        Task<PaginatedList<Discount>> GetAsync(Query<Discount> query);
    }
}
