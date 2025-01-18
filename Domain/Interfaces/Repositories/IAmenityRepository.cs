using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;
namespace Domain.Interfaces.Repositories
{
    public interface IAmenityRepository
    {
        Task<PaginatedList<Amenity>> GetAsync(Query<Amenity> query);
        Task<Amenity?> GetByIdAsync(Guid id);
        Task<bool> ExistAsync(Expression<Func<Amenity, bool>> predicate);
        Task<Amenity> CreateAsync(Amenity amenity);
        Task UpdtaeAsync(Amenity amenity);
    }
}
