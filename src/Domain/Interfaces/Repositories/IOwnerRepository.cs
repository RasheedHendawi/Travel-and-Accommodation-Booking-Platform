using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface IOwnerRepository
    {
        Task<PaginatedList<Owner>> GetAsync(Query<Owner> query);

        Task<Owner?> GetByIdAsync(Guid id);
        Task<bool> ExistsAsync(Expression<Func<Owner, bool>> predicate);

        Task<Owner> CreateAsync(Owner owner);

        Task UpdateAsync(Owner owner);
    }
}
