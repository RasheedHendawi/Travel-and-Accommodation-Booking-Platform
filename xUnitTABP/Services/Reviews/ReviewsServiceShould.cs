using Application.DTOs.Hotels;
using Application.DTOs.Reviews;
using Application.DTOs.Shared;
using Application.Services.Reviews;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Moq;
using System.Linq.Expressions;

namespace Application.Tests.Services.Reviews
{
    public class ReviewsServiceShould
    {
        private readonly Mock<IReviewRepository> _reviewRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IHttpUserContextAccessor> _userAccessorMock;
        private readonly ReviewService _reviewService;

        public ReviewsServiceShould()
        {
            _reviewRepositoryMock = new Mock<IReviewRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _userAccessorMock = new Mock<IHttpUserContextAccessor>();

            _reviewService = new ReviewService(
                _reviewRepositoryMock.Object,
                _hotelRepositoryMock.Object,
                _bookingRepositoryMock.Object,
                _userRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object,
                _userAccessorMock.Object
            );
        }

        [Fact]
        public async Task ReturnReviewWhenReviewExists()
        {
            var hotelId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var review = new Review { Id = reviewId, HotelId = hotelId };
            var reviewResponse = new ReviewResponse { Id = reviewId };

            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
                .ReturnsAsync(true);
            _reviewRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId, reviewId))
                .ReturnsAsync(review);
            _mapperMock.Setup(mapper => mapper.Map<ReviewResponse>(review))
                .Returns(reviewResponse);

            var result = await _reviewService.GetReviewByIdAsync(hotelId, reviewId);

            Assert.NotNull(result);
            Assert.Equal(reviewId, result.Id);
        }

        [Fact]
        public async Task CreateReview_WhenValidRequest()
        {
            var hotelId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new ReviewCreationRequest { Rating = 5 };
            var newReview = new Review { Id = Guid.NewGuid(), Rating = 5, GuestId = userId, HotelId = hotelId };
            var reviewResponse = new ReviewResponse { Id = newReview.Id };

            _userAccessorMock.Setup(accessor => accessor.Id).Returns(userId);
            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
                .ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.ExistsByIdAsync(userId))
                .ReturnsAsync(true);
            _bookingRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Booking, bool>>>()))
                .ReturnsAsync(true);
            _reviewRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Review, bool>>>()))
                .ReturnsAsync(false);
            _mapperMock.Setup(mapper => mapper.Map<Review>(request))
                .Returns(newReview);
            _reviewRepositoryMock.Setup(repo => repo.CreateAsync(newReview))
                .ReturnsAsync(newReview);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(1);
            _mapperMock.Setup(mapper => mapper.Map<ReviewResponse>(newReview))
                .Returns(reviewResponse);

            var result = await _reviewService.CreateReviewAsync(hotelId, request);

            Assert.NotNull(result);
            Assert.Equal(newReview.Id, result.Id);
        }

        [Fact]
        public async Task UpdateReviewWhenValidRequest()
        {
            var hotelId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var request = new ReviewUpdateRequest { Rating = 4 };
            var existingReview = new Review { Id = reviewId, Rating = 5, GuestId = userId, HotelId = hotelId };

            _userAccessorMock.Setup(accessor => accessor.Id).Returns(userId);
            _userAccessorMock.Setup(accessor => accessor.Role).Returns(UserRoles.Guest);
            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>())).ReturnsAsync(true);
            _userRepositoryMock.Setup(repo => repo.ExistsByIdAsync(userId)).ReturnsAsync(true);
            _reviewRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId, reviewId, userId)).ReturnsAsync(existingReview);
            _mapperMock.Setup(mapper => mapper.Map(request, existingReview));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

            await _reviewService.UpdateReviewAsync(hotelId, reviewId, request);

            _reviewRepositoryMock.Verify(repo => repo.UpdateAsync(existingReview), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task DeleteReview_WhenReviewExists()
        {
            var hotelId = Guid.NewGuid();
            var reviewId = Guid.NewGuid();
            var userId = Guid.NewGuid();
            var existingReview = new Review { Id = reviewId, GuestId = userId, HotelId = hotelId };

            _userAccessorMock.Setup(accessor => accessor.Id).Returns(userId);
            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>())).ReturnsAsync(true);
            _reviewRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId, reviewId, userId)).ReturnsAsync(existingReview);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);


            await _reviewService.DeleteReviewAsync(hotelId, reviewId);

            _reviewRepositoryMock.Verify(repo => repo.DeleteAsync(reviewId), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ReturnPaginatedReviews()
        {
            var hotelId = Guid.NewGuid();
            var request = new ReviewsGetRequest { PageNumber = 1, PageSize = 2, SortOrder = SortMethod.Ascending };
            var reviews = new List<Review>
            {
                new() { Id = Guid.NewGuid(), HotelId = hotelId, Rating = 5 },
                new() { Id = Guid.NewGuid(), HotelId = hotelId, Rating = 4 }
            };
            var paginatedReviews = new PaginatedList<Review>(reviews, new PaginationMetaData(1, 1, 2));

            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
                .ReturnsAsync(true);
            _reviewRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Query<Review>>()))
                .ReturnsAsync(paginatedReviews);
            _mapperMock.Setup(mapper => mapper.Map<ReviewResponse>(It.IsAny<Review>()))
                .Returns(new ReviewResponse());

            var result = await _reviewService.GetReviewsForHotelAsync(hotelId, request);
            var itemsCount = result.Items.Count();

            Assert.NotNull(result);
            Assert.Equal(2, itemsCount);
        }
    }
}



