using Application.Contracts;
using Application.DTOs.Hotels;
using Application.DTOs.Reviews;
using Application.DTOs.Shared;
using Application.Exceptions.BookingExceptions;
using Application.Exceptions.HotelExceptions;
using Application.Exceptions.UserExceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Security.Claims;
using System.Threading;

namespace Application.Services.Reviews
{
    public class ReviewService : IReviewService
    {
        private readonly IReviewRepository _reviewRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHttpUserContextAccessor _userAccessor;

        public ReviewService(
            IReviewRepository reviewRepository,
            IHotelRepository hotelRepository,
            IBookingRepository bookingRepository,
            IUserRepository userRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper,
            IHttpUserContextAccessor userAccessor)
        {
            _reviewRepository = reviewRepository;
            _hotelRepository = hotelRepository;
            _bookingRepository = bookingRepository;
            _userRepository = userRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _userAccessor = userAccessor;
        }

        public async Task<PaginatedList<ReviewResponse>> GetReviewsForHotelAsync(Guid hotelId, ReviewsGetRequest request)
        {
            if (!await _hotelRepository.ExistsAsync(h => h.Id == hotelId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            var query = new Query<Review>(
                r => r.HotelId == hotelId,
                request.SortOrder ?? SortMethod.Ascending,
                request.SortColumn,
                request.PageNumber,
                request.PageSize);

            var reviews = await _reviewRepository.GetAsync(query);
            var mappedReviews = reviews.Items
                .Select(review => _mapper.Map<ReviewResponse>(review))
                .ToList();

            return new PaginatedList<ReviewResponse>(mappedReviews, reviews.PaginationMetadata);
        }

        public async Task<ReviewResponse> GetReviewByIdAsync(Guid hotelId, Guid reviewId)
        {
            if (!await _hotelRepository.ExistsAsync(h => h.Id == hotelId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            var review = await _reviewRepository.GetByIdAsync(hotelId, reviewId)
                         ?? throw new HotelNotFoundException("Hotel not found.");

            return _mapper.Map<ReviewResponse>(review);
        }

        public async Task<ReviewResponse> CreateReviewAsync(Guid hotelId, ReviewCreationRequest request)
        {
            var userId = _userAccessor.Id;

            if (!await _hotelRepository.ExistsAsync(h => h.Id == hotelId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            if (!await _userRepository.ExistsByIdAsync(userId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            if (!await _bookingRepository.ExistsAsync(b => b.HotelId == hotelId && b.GuestId == userId))
            {
                throw new BookingNotFoundException("Booking not found for the guest.");
            }

            if (await _reviewRepository.ExistsAsync(r => r.GuestId == userId && r.HotelId == hotelId))
            {
                throw new GuestAlreadyReviewedException("Guest already reviewed this hotel.");
            }

            var newReview = _mapper.Map<Review>(request);
            newReview.GuestId = userId;
            newReview.HotelId = hotelId;
            var TotalRating = await _reviewRepository.GetTotalRatingForHotelAsync(hotelId);
            var TotalReviews = await _reviewRepository.GetReviewCountForHotelAsync(hotelId);
            TotalRating += newReview.Rating;
            TotalReviews++;
            var finalRating = 1.0 * TotalRating / TotalReviews;
            await _hotelRepository.UpdateReviewById(hotelId, finalRating);
            var createdReview = await _reviewRepository.CreateAsync(newReview);
            await _unitOfWork.SaveChangesAsync();

            return _mapper.Map<ReviewResponse>(createdReview);
        }

        public async Task UpdateReviewAsync(Guid hotelId, Guid reviewId, ReviewUpdateRequest request)
        {
            var userId = _userAccessor.Id;
            if (!await _hotelRepository.ExistsAsync(h => h.Id == hotelId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            if (!await _userRepository.ExistsByIdAsync(userId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            if (_userAccessor.Role != UserRoles.Guest)
            {
                throw new ForbiddenUserException("Not guest !");
            }

            var review = await _reviewRepository.GetByIdAsync(hotelId, reviewId, userId)
                         ?? throw new HotelNotFoundException("Hotel not found.");
            var TotalRating = await _reviewRepository.GetTotalRatingForHotelAsync(hotelId);
            var TotalReviews = await _reviewRepository.GetReviewCountForHotelAsync(hotelId);
            TotalRating += request.Rating - review.Rating;
            var finalRating = 1.0 * TotalRating / TotalReviews;
            await _hotelRepository.UpdateReviewById(hotelId, finalRating);
            _mapper.Map(request, review);
            await _reviewRepository.UpdateAsync(review);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteReviewAsync(Guid hotelId, Guid reviewId)
        {
            var userId = _userAccessor.Id;

            if (!await _hotelRepository.ExistsAsync(h => h.Id == hotelId))
            {
                throw new HotelNotFoundException("Hotel not found.");
            }

            var review = await _reviewRepository.GetByIdAsync(hotelId, reviewId, userId)
                         ?? throw new ReviewNotFoundException("Review not found!");

            await _reviewRepository.DeleteAsync(reviewId);
            await _unitOfWork.SaveChangesAsync();
        }

    }
}
