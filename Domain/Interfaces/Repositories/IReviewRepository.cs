using Domain.Entities;
using Domain.Models;
using System.Linq.Expressions;


namespace Domain.Interfaces.Repositories
{
    public interface IReviewRepository
    {
        Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate);
        public Task<PaginatedList<Review>> GetAsync(Query<Review> query);

        Task<Review?> GetByIdAsync(Guid hotelId, Guid reviewId);

        Task<Review> CreateAsync(Review review);

        Task<Review?> GetByIdAsync(Guid reviewId, Guid hotelId, Guid guestId);

        Task DeleteAsync(Guid reviewId);

        Task<int> GetTotalRatingForHotelAsync(Guid hotelId);

        Task<int> GetReviewCountForHotelAsync(Guid hotelId);

        Task UpdateAsync(Review review);
    }
}
