using Application.DTOs.Hotels;
using Application.DTOs.Reviews;
using Domain.Models;

namespace Application.Contracts
{
    public interface IReviewService
    {
        Task<PaginatedList<ReviewResponse>> GetReviewsForHotelAsync(Guid hotelId, ReviewsGetRequest request);
        Task<ReviewResponse> GetReviewByIdAsync(Guid hotelId, Guid reviewId);
        Task<ReviewResponse> CreateReviewAsync(Guid hotelId, ReviewCreationRequest request);
        Task UpdateReviewAsync(Guid hotelId, Guid reviewId, ReviewUpdateRequest request);
        Task DeleteReviewAsync(Guid hotelId, Guid reviewId);
    }
}
