using Domain.Entities;
using Domain.Enums;
using Domain.Models;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Text;

namespace Domain.Interfaces.Repositories
{
    public interface IHotelRepository
    {
        Task<Hotel?> GetByIdAsync(Guid id, bool includeCity = false, bool includeThmbnail = false, bool includeGallary = false);
        Task<Hotel> CreateAsync(Hotel hotel);
        Task UpdateAsync(Hotel hotel);
        Task DeleteAsync(Guid id);
        Task<PaginatedList<HotelSearchResult>> GetForSearchAsync(Query<Hotel> query);
        Task UpdateReviewById(Guid id, double newRating);
        Task<bool> ExistsAsync(Expression<Func<Hotel, bool>> expression);
        Task<PaginatedList<HotelForManagement>> GetForManagementAsync(Query<Hotel> query);

    }
}
