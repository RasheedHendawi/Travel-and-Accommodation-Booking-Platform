using Application.Contracts;
using Application.DTOs.Bookings;
using Application.DTOs.Shared;
using Application.Exceptions.BookingExceptions;
using Application.Exceptions.UserExceptions;
using Application.Services.Bookings;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Moq;

namespace Application.Services.Bookings;
public class BookingServiceTests
{
    private readonly Mock<IBookingRepository> _bookingRepositoryMock = new();
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IRoomRepository> _roomRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IUserRepository> _userRepositoryMock = new();
    private readonly Mock<IPdfService> _pdfServiceMock = new();
    private readonly Mock<IEmailService> _emailServiceMock = new();
    private readonly Mock<IHttpUserContextAccessor> _userContextMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly BookingService _service;

    public BookingServiceTests()
    {
        _service = new BookingService(
            _bookingRepositoryMock.Object,
            _hotelRepositoryMock.Object,
            _roomRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _userRepositoryMock.Object,
            _pdfServiceMock.Object,
            _emailServiceMock.Object,
            _userContextMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task ThrowExceptionWhenUserNotFound()
    {
        _userContextMock.Setup(uc => uc.Id).Returns(Guid.NewGuid());
        _userRepositoryMock.Setup(ur => ur.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(false);

        var request = new BookingCreationRequest { HotelId = Guid.NewGuid(), RoomIds = new List<Guid>() };

        await Assert.ThrowsAsync<UserNotFoundException>(() => _service.CreateBookingAsync(request));
    }

    [Fact]
    public async Task ThrowExceptionWhenUserNotGuest()
    {
        _userContextMock.Setup(uc => uc.Id).Returns(Guid.NewGuid());
        _userContextMock.Setup(uc => uc.Role).Returns(UserRoles.Admin);
        _userRepositoryMock.Setup(ur => ur.ExistsByIdAsync(It.IsAny<Guid>())).ReturnsAsync(true);

        var request = new BookingCreationRequest { HotelId = Guid.NewGuid(), RoomIds = new List<Guid>() };

        await Assert.ThrowsAsync<ForbiddenUserException>(() => _service.CreateBookingAsync(request));
    }

    [Fact]
    public async Task ReturnBookingWhenBookingExists()
    {
        var bookingId = Guid.NewGuid();
        var booking = new Booking { Id = bookingId };

        _bookingRepositoryMock.Setup(br => br.GetByIdAsync(bookingId, It.IsAny<Guid>(), false))
            .ReturnsAsync(booking);
        _mapperMock.Setup(m => m.Map<BookingResponse>(It.IsAny<Booking>()))
            .Returns(new BookingResponse());

        var result = await _service.GetBookingAsync(bookingId);

        Assert.NotNull(result);
    }

    [Fact]
    public async Task ThrowExceptionWhenBookingDoesNotExist()
    {
        _bookingRepositoryMock.Setup(br => br.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(null as Booking);

        await Assert.ThrowsAsync<BookingNotFoundException>(() => _service.DeleteBookingAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task ThrowExceptionWhenCheckInPassed()
    {
        var booking = new Booking { CheckIn = DateOnly.FromDateTime(DateTime.UtcNow.AddDays(-1)) };
        _bookingRepositoryMock.Setup(br => br.GetByIdAsync(It.IsAny<Guid>()))
            .ReturnsAsync(booking);

        await Assert.ThrowsAsync<BookingCancellationException>(() => _service.DeleteBookingAsync(Guid.NewGuid()));
    }
}
