using Application.DTOs.Rooms;
using Application.Services.Rooms;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Moq;
using System.Linq.Expressions;


namespace Application.Tests.Services.Rooms
{
    public class RoomsServiceShould
    {
        private readonly Mock<IRoomRepository> _roomRepositoryMock;
        private readonly Mock<IBookingRepository> _bookingRepositoryMock;
        private readonly Mock<IRoomClassRepository> _roomClassRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly RoomService _roomService;

        public RoomsServiceShould()
        {
            _roomRepositoryMock = new Mock<IRoomRepository>();
            _bookingRepositoryMock = new Mock<IBookingRepository>();
            _roomClassRepositoryMock = new Mock<IRoomClassRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _roomService = new RoomService(
                _roomRepositoryMock.Object,
                _bookingRepositoryMock.Object,
                _roomClassRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task ReturnPaginatedListWhenValidRequest()
        {
            var roomClassId = Guid.NewGuid();
            var request = new RoomsGetRequest
            {
                PageNumber = 1,
                PageSize = 2,
                SortOrder = SortMethod.Ascending
            };

            var rooms = new List<RoomForManagement>
            {
                new() { Id = Guid.NewGuid(), Number = "101", RoomClassId = roomClassId },
                new() { Id = Guid.NewGuid(), Number = "102", RoomClassId = roomClassId }
            };

            var paginatedRooms = new PaginatedList<RoomForManagement>(rooms, new PaginationMetaData(2, 1, 2));

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(true);
            _roomRepositoryMock.Setup(repo => repo.GetForManagementAsync(It.IsAny<Query<Room>>()))
                .ReturnsAsync(paginatedRooms);
            _mapperMock.Setup(mapper => mapper.Map<PaginatedList<RoomForManagementResponse>>(It.IsAny<PaginatedList<RoomForManagement>>()))
                .Returns(new PaginatedList<RoomForManagementResponse>([], new PaginationMetaData(2, 1, 2)));

            var result = await _roomService.GetRoomsForManagementAsync(roomClassId, request);

            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task ThrowExceptionWhenRoomClassNotFound()
        {

            var roomClassId = Guid.NewGuid();
            var request = new RoomsGetRequest();

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(false);

            await Assert.ThrowsAsync<Exception>(() => _roomService.GetRoomsForManagementAsync(roomClassId, request));
        }

        [Fact]
        public async Task CreateRoomWhenValidRequest()
        {
            var roomClassId = Guid.NewGuid();
            var request = new RoomCreationRequest { Number = "101" };
            var createdRoom = new Room { Id = Guid.NewGuid(), Number = "101", RoomClassId = roomClassId };

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(true);
            _roomRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
                .ReturnsAsync(false);
            _mapperMock.Setup(mapper => mapper.Map<Room>(It.IsAny<CreateRoomHandler>()))
                .Returns(createdRoom);
            _roomRepositoryMock.Setup(repo => repo.CreateAsync(It.IsAny<Room>()))
                .ReturnsAsync(createdRoom);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(1);


            var result = await _roomService.CreateRoomAsync(roomClassId, request);

            Assert.Equal(createdRoom.Id, result);
        }

        [Fact]
        public async Task ThrowExceptionWhenDuplicateRoomNumber()
        {

            var roomClassId = Guid.NewGuid();
            var request = new RoomCreationRequest { Number = "101" };

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(true);
            _roomRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _roomService.CreateRoomAsync(roomClassId, request));
        }

        [Fact]
        public async Task ThrowExceptionWhenRoomHasBookings()
        {
            var roomClassId = Guid.NewGuid();
            var roomId = Guid.NewGuid();

            _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
                .ReturnsAsync(true);
            _roomRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Room, bool>>>()))
                .ReturnsAsync(true);
            _bookingRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Booking, bool>>>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _roomService.DeleteRoomAsync(roomClassId, roomId));
        }
    }
}
