using Application.DTOs.Hotels;
using Application.Exceptions.HotelExceptions;
using Application.Services.Hotels;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Moq;
using System.Linq.Expressions;

namespace Application.Tests.Services.Hotels;
public class HotelServiceShould
{
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly Mock<IOwnerRepository> _ownerRepositoryMock = new();
    private readonly Mock<IImageRepository> _imageRepositoryMock = new();
    private readonly Mock<IRoomClassRepository> _roomClassRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly HotelsService _service;

    public HotelServiceShould()
    {
        _service = new HotelsService(
            _hotelRepositoryMock.Object,
            _cityRepositoryMock.Object,
            _ownerRepositoryMock.Object,
            _imageRepositoryMock.Object,
            _roomClassRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task ReturnsHotelResponse()
    {
        var hotelId = Guid.NewGuid();
        var hotel = new Hotel { Id = hotelId, Name = "Test Hotel" };
        var hotelResponse = new HotelGetResponse { Id = hotelId, Name = "Test Hotel" };

        _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId, true, true, true))
            .ReturnsAsync(hotel);
        _mapperMock.Setup(mapper => mapper.Map<HotelGetResponse>(hotel)).Returns(hotelResponse);

        var result = await _service.GetHotelByIdAsync(hotelId);

        Assert.NotNull(result);
        Assert.Equal(hotelId, result.Id);
    }

    [Fact]
    public async Task ThrowsExceptionWhenHotelNotFound()
    {
        var hotelId = Guid.NewGuid();
        _hotelRepositoryMock.Setup(repo => repo.GetByIdAsync(hotelId, true, true, true))
            .ReturnsAsync(null as Hotel);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _service.GetHotelByIdAsync(hotelId));
    }

    [Fact]
    public async Task ReturnsHotelIdWhenValidRequest()
    {
        var request = new HotelCreationRequest { CityId = Guid.NewGuid(), OwnerId = Guid.NewGuid(), Latitude = 10, Longitude = 20 };
        var hotel = new Hotel { Id = Guid.NewGuid() };

        _cityRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<City, bool>>>()))
            .ReturnsAsync(true);
        _ownerRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Owner, bool>>>()))
            .ReturnsAsync(true);
        _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
            .ReturnsAsync(false);
        _mapperMock.Setup(mapper => mapper.Map<Hotel>(request)).Returns(hotel);
        _hotelRepositoryMock.Setup(repo => repo.CreateAsync(hotel)).ReturnsAsync(hotel);

        var result = await _service.CreateHotelAsync(request);

        Assert.Equal(hotel.Id, result);
    }

    [Fact]
    public async Task ThrowsExceptionWhenCityNotFound()
    {
        var request = new HotelCreationRequest { CityId = Guid.NewGuid() };
        _cityRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<City, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<CityNotFoundException>(() => _service.CreateHotelAsync(request));
    }

    [Fact]
    public async Task DeletesHotelWhenHotelExists()
    {
        var hotelId = Guid.NewGuid();
        _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
            .ReturnsAsync(true);
        _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
            .ReturnsAsync(false);

        await _service.DeleteHotelAsync(hotelId);

        _hotelRepositoryMock.Verify(repo => repo.DeleteAsync(hotelId), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ThrowsExceptionWhenHotlNotFound()
    {
        var hotelId = Guid.NewGuid();
        _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<HotelNotFoundException>(() => _service.DeleteHotelAsync(hotelId));
    }
}
