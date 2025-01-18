using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;

namespace Domain.Interfaces.Repositories
{
    public interface ICityRepository
    {
        Task<IEnumerable<City>> GetMostVisitedAsync(int count);
        Task CreateAsync(City city);
        Task<City?> GetByIdAsync(Guid id);
        Task DeleteAsync(Guid id);
        Task UpdateAsync(City city);
        Task<PaginatedList<CityForManagement>> GetForManagement(Query<City> query);
        Task<bool> ExistsAsync(Expression<Func<City, bool>> expression);

    }
}
