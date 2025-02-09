using Application.DTOs.Images;
using Application.DTOs.RoomClass;
using Application.Exceptions.GeneralExceptions;
using Application.Exceptions.RoomExceptions;
using Application.Services.RoomClasses;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using Moq;

using System.Linq.Expressions;

namespace Application.Tests.Services.RoomClasses
{
    public class RoomClassServiceTests
    {
        private readonly RoomClassService _roomClassService;
        private readonly Mock<IRoomClassRepository> _roomClassRepositoryMock;
        private readonly Mock<IHotelRepository> _hotelRepositoryMock;
        private readonly Mock<IAmenityRepository> _amenityRepositoryMock;
        private readonly Mock<IImageRepository> _imageRepositoryMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;

        public RoomClassServiceTests()
        {
            _roomClassRepositoryMock = new Mock<IRoomClassRepository>();
            _hotelRepositoryMock = new Mock<IHotelRepository>();
            _amenityRepositoryMock = new Mock<IAmenityRepository>();
            _imageRepositoryMock = new Mock<IImageRepository>();
            _mapperMock = new Mock<IMapper>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();

            _roomClassService = new RoomClassService(
                _roomClassRepositoryMock.Object,
                _hotelRepositoryMock.Object,
                _amenityRepositoryMock.Object,
                _imageRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task ReturnPaginatedListWithEmptyData()
        {

            var request = new ResourcesQueryRequest { PageNumber = 1, PageSize = 2, SortOrder = SortMethod.Ascending };
            var roomClasses = new List<RoomClass>
            {
                new() { Id = Guid.NewGuid(), Name = "A-Class", HotelId = Guid.NewGuid() },
                new() { Id = Guid.NewGuid(), Name = "B-Class", HotelId = Guid.NewGuid() }
            };
            var paginatedRoomClasses = new PaginatedList<RoomClass>(roomClasses, new PaginationMetaData(1, 1, 2));

            _roomClassRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Query<RoomClass>>(), false))
                .ReturnsAsync(paginatedRoomClasses);
            _mapperMock.Setup(mapper => mapper.Map<PaginatedList<RoomClassForManagementResponse>>(paginatedRoomClasses))
                .Returns(new PaginatedList<RoomClassForManagementResponse>([], new PaginationMetaData(1, 1, 2)));

            var result = await _roomClassService.GetAllRoomClassesAsync(request);

            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task ReturnRoomClassWhenFound()
        {
            var roomClassId = Guid.NewGuid();
            var roomClass = new RoomClass { Id = roomClassId, Name = "Class-A", HotelId = Guid.NewGuid() };

            _roomClassRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId))
                .ReturnsAsync(roomClass);
            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(true);
            _mapperMock.Setup(mapper => mapper.Map<RoomClassForManagementResponse>(roomClass))
                .Returns(new RoomClassForManagementResponse { RoomClassId = roomClassId, Name = "Class-A" });

            var result = await _roomClassService.GetRoomClassByIdAsync(roomClassId);

            Assert.NotNull(result);
            Assert.Equal(roomClassId, result.RoomClassId);
        }

        [Fact]
        public async Task ThrowExceptionWhenNotFound()
        {
            var roomClassId = Guid.NewGuid();
            _roomClassRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId))
                .ReturnsAsync(null as RoomClass);
            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<EntityNotFoundException>(() => _roomClassService.GetRoomClassByIdAsync(roomClassId));
        }

        [Fact]
        public async Task ReturnRoomClassIdWhenValid()
        {
            var roomClassId = Guid.NewGuid();
            var hotelId = Guid.NewGuid();
            var roomClassRequest = new RoomClassCreationRequest { Name = "Class-A", HotelId = hotelId, AmenitiesId = [Guid.NewGuid()] };
            var roomClassEntity = new RoomClass { Id = roomClassId, Name = "Class-A", HotelId = hotelId };

            _hotelRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Hotel, bool>>>())).ReturnsAsync(true);
            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(false);
            _amenityRepositoryMock.Setup(repo => repo.ExistAsync(It.IsAny<Expression<Func<Amenity, bool>>>()))
                .ReturnsAsync(true);
            _mapperMock.Setup(mapper => mapper.Map<RoomClass>(roomClassRequest)).Returns(roomClassEntity);
            _roomClassRepositoryMock.Setup(repo => repo.CreateAsync(roomClassEntity)).ReturnsAsync(roomClassEntity);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

            var result = await _roomClassService.CreateRoomClassAsync(roomClassRequest);

            Assert.Equal(roomClassId, result);
        }

        [Fact]
        public async Task DeleteRoomClassWhenExistsWithNoExceptions()
        {
            var roomClassId = Guid.NewGuid();
            var roomClass = new RoomClass { Id = roomClassId, Name = "Class-A", HotelId = Guid.NewGuid() };

            _roomClassRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId))
                .ReturnsAsync(roomClass);
            _roomClassRepositoryMock.Setup(repo => repo.DeleteAsync(roomClassId))
                .Returns(Task.CompletedTask);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

            var exception = await Record.ExceptionAsync(() =>
            _roomClassService.DeleteRoomClassAsync(roomClassId));
            Assert.Null(exception);
        }

        [Fact]
        public async Task ThrowExceptionWhenNotFoundForDeletion()
        {
            var roomClassId = Guid.NewGuid();
            _roomClassRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId))
                .ReturnsAsync(null as RoomClass);

            await Assert.ThrowsAsync<RoomClassNotFoundException>(() => _roomClassService.DeleteRoomClassAsync(roomClassId));
        }

        [Fact]
        public async Task ShouldAddImageWhenRoomClassExists()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[10]));
            fileMock.Setup(f => f.Length).Returns(10);//becaues I can't do straight array of bytes

            var roomClassId = Guid.NewGuid();
            var imageRequest = new ImageCreationRequest { Image = fileMock.Object };

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(rc => rc.Id == roomClassId))
                .ReturnsAsync(true);
            _imageRepositoryMock
                .Setup(repo => repo.CreateAsync(It.IsAny<IFormFile>(), roomClassId, ImageType.Gallery))
                .ReturnsAsync(new Image { Id = Guid.NewGuid(), EntityId = roomClassId, Type = ImageType.Gallery });

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

            var exception = await Record.ExceptionAsync(() =>
                    _roomClassService.AddImageToRoomClassAsync(roomClassId, imageRequest));
            Assert.Null(exception);
        }

        [Fact]
        public async Task ThrowException_WhenRoomClassDoesNotExist()
        {
            var fileMock = new Mock<IFormFile>();
            fileMock.Setup(f => f.OpenReadStream()).Returns(new MemoryStream(new byte[10]));
            fileMock.Setup(f => f.Length).Returns(10);

            var roomClassId = Guid.NewGuid();
            var imageRequest = new ImageCreationRequest { Image = fileMock.Object };

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(rc => rc.Id == roomClassId))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<RoomClassNotFoundException>(() => _roomClassService.AddImageToRoomClassAsync(roomClassId, imageRequest));
        }
        [Fact]
        public async Task ThrowExceptionWhenRoomClassNotFoundWhileUpdating()
        {
            var roomClassId = Guid.NewGuid();
            var updatedRoomClass = new RoomClassUpdateRequest { Name = "Class-A" };
            _roomClassRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId))
                .ReturnsAsync(null as RoomClass);
            await Assert.ThrowsAsync<RoomClassNotFoundException>(() =>  _roomClassService.UpdateRoomClassAsync(roomClassId, updatedRoomClass));
        }
        [Fact]
        public async Task ThrowExceptionWhenRoomClassNameFoundWithHotelWhileUpdating()
        {
            var roomClassId = Guid.NewGuid();
            var updatedRoomClass = new RoomClassUpdateRequest { Name = "Class-A" };
            var existingRoomClass = new RoomClass { Id = roomClassId, Name = "Class-B", HotelId = Guid.NewGuid() };
            _roomClassRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId))
                .ReturnsAsync(existingRoomClass);
            _roomClassRepositoryMock.Setup(repo =>repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(true);
            await Assert.ThrowsAsync<RoomClassWithHotelFound>(() => _roomClassService.UpdateRoomClassAsync(roomClassId, updatedRoomClass));
        }
    }

}