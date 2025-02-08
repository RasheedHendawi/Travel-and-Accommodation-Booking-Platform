using Application.DTOs.Cities;
using Application.DTOs.Images;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Linq.Expressions;
using System.Text;

namespace Application.Services.Cities;
public class CityServiceShould
{
    private readonly Mock<ICityRepository> _cityRepositoryMock = new();
    private readonly Mock<IHotelRepository> _hotelRepositoryMock = new();
    private readonly Mock<IImageRepository> _imageRepositoryMock = new();
    private readonly Mock<IUnitOfWork> _unitOfWorkMock = new();
    private readonly Mock<IMapper> _mapperMock = new();
    private readonly CityService _cityService;

    public CityServiceShould()
    {
        _cityService = new CityService(
            _cityRepositoryMock.Object,
            _hotelRepositoryMock.Object,
            _imageRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object
        );
    }

    [Fact]
    public async Task ReturnsPaginatedList()
    {
        var request = new CitiesGetHandler { PageNumber = 1, PageSize = 10 };

        var cityForManagementList = new PaginatedList<CityForManagement>(
            new List<CityForManagement>(),
            new PaginationMetaData(1, 10, 1)
        );

        _cityRepositoryMock.Setup(repo => repo.GetForManagement(It.IsAny<Query<City>>()))
            .ReturnsAsync(cityForManagementList);

        var result = await _cityService.GetCitiesForManagementAsync(request);

        Assert.NotNull(result);
        Assert.IsType<PaginatedList<CityForManagementResponse>>(result);
    }


    [Fact]
    public async Task ThrowsExceptionWhenInvalidCount()
    {
        var request = new TrendingCityRequest { Count = 0 };

        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await _cityService.GetTrendingCitiesAsync(request));
    }

    [Fact]
    public async Task CallsRepositoryWhenValidRequest()
    {
        var request = new CityCreationRequest { PostOffice = "12345" };
        _cityRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<City, bool>>>()))
            .ReturnsAsync(false);

        await _cityService.CreateCityAsync(request);

        _cityRepositoryMock.Verify(repo => repo.CreateAsync(It.IsAny<City>()), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task UpdatesCityWhenCityExists()
    {
        var city = new City { Id = Guid.NewGuid(), PostOffice = "12345" };
        var request = new CityUpdateRequest { PostOffice = "54321" };

        _cityRepositoryMock.Setup(repo => repo.GetByIdAsync(city.Id)).ReturnsAsync(city);
        _cityRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<City, bool>>>()))
            .ReturnsAsync(false);

        await _cityService.UpdateCityAsync(city.Id, request);

        _cityRepositoryMock.Verify(repo => repo.UpdateAsync(city), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }

    [Fact]
    public async Task ThrowsExceptionCityNotExists()
    {
        _cityRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<City, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<Exception>(async () =>
            await _cityService.DeleteCityAsync(Guid.NewGuid()));
    }

    [Fact]
    public async Task SetsThumbnailWhenCityExists()
    {
        var cityId = Guid.NewGuid();
        var fileMock = new Mock<IFormFile>();
        var content = "FakeImageData";
        var fileName = "image.jpg";
        var stream = new MemoryStream(Encoding.UTF8.GetBytes(content));
        fileMock.Setup(f => f.OpenReadStream()).Returns(stream);
        fileMock.Setup(f => f.FileName).Returns(fileName);
        fileMock.Setup(f => f.Length).Returns(stream.Length);
        fileMock.Setup(f => f.ContentType).Returns("image/jpeg");

        var request = new ImageCreationRequest { Image = fileMock.Object };
        _cityRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<City, bool>>>()))
            .ReturnsAsync(true);

        await _cityService.SetCityThumbnailAsync(cityId, request);

        _imageRepositoryMock.Verify(repo => 
        repo.DeleteAsync(cityId, ImageType.Thumbnail), Times.Once);
        _imageRepositoryMock.Verify(repo => 
        repo.CreateAsync(request.Image, cityId, ImageType.Thumbnail), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
