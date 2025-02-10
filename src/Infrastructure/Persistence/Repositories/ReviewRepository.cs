using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Models;
using Infrastructure.Persistence.ContextDb;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Infrastructure.Utilites;
namespace Infrastructure.Persistence.Repositories
{
    public class ReviewRepository(HotelBookingPlatformDbContext context) : IReviewRepository
    {
        public async Task<Review> CreateAsync(Review review)
        {
            ArgumentNullException.ThrowIfNull(review);

            var addedReview = await context.Reviews.AddAsync(review);

            return addedReview.Entity;
        }

        public async Task DeleteAsync(Guid reviewId)
        {
            if (!await context.Reviews.AnyAsync(r => r.Id == reviewId))
            {
                throw new DirectoryNotFoundException("Review with this Id not Found");
            }

            var entity = context.ChangeTracker.Entries<Review>()
              .FirstOrDefault(e => e.Entity.Id == reviewId)?.Entity
              ?? new Review { Id = reviewId };

            context.Reviews.Remove(entity);
        }

        public async Task<bool> ExistsAsync(Expression<Func<Review, bool>> predicate)
        {
            return await context.Reviews.AnyAsync(predicate);
        }

        public async Task<PaginatedList<Review>> GetAsync(Query<Review> query)
        {
            var queryable = context.Reviews
              .Where(query.Filter)
              .Sort(SortingExpressions.GetReviewSortExpression(query.SortColumn), query.SortOrder);

            var itemsToReturn = await queryable
              .GetPage(query.PageNumber, query.PageSize)
              .AsNoTracking()
              .ToListAsync();

            return new PaginatedList<Review>(
              itemsToReturn,
              await queryable.GetPaginationDataAsync(
                query.PageNumber,
                query.PageSize));
        }

        public async Task<Review?> GetByIdAsync(Guid hotelId, Guid reviewId)
        {
            return await context.Reviews
                  .FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == hotelId);
        }

        public async Task<Review?> GetByIdAsync(Guid reviewId, Guid hotelId, Guid guestId)
        {
            return await context.Reviews
              .FirstOrDefaultAsync(r => r.Id == reviewId && r.HotelId == hotelId && r.GuestId == guestId);
        }

        public async Task<int> GetReviewCountForHotelAsync(Guid hotelId)
        {
            return await context.Reviews
              .Where(r => r.HotelId == hotelId)
              .CountAsync();
        }

        public async Task<int> GetTotalRatingForHotelAsync(Guid hotelId)
        {
            return await context.Reviews
              .Where(r => r.HotelId == hotelId)
              .SumAsync(r => r.Rating);
        }

        public async Task UpdateAsync(Review review)
        {
            ArgumentNullException.ThrowIfNull(review);

            if (!await context.Reviews.AnyAsync(r => r.Id == review.Id))
            {
                throw new Exception("Review with this Id not Found");
            }

            context.Reviews.Update(review);
        }
    }
}
