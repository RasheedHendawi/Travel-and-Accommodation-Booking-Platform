using Application.DTOs.Discounts;
using Application.Exceptions.RoomExceptions;
using Application.Services.Discounts;
using AutoMapper;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Moq;
using System.Linq.Expressions;

namespace Application.Tests.Services.Discounts;
public class DiscountsServiceShould
{
    private readonly Mock<IDiscountRepository> _discountRepositoryMock;
    private readonly Mock<IRoomClassRepository> _roomClassRepositoryMock;
    private readonly Mock<IUnitOfWork> _unitOfWorkMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly DiscountService _discountService;

    public DiscountsServiceShould()
    {
        _discountRepositoryMock = new Mock<IDiscountRepository>();
        _roomClassRepositoryMock = new Mock<IRoomClassRepository>();
        _unitOfWorkMock = new Mock<IUnitOfWork>();
        _mapperMock = new Mock<IMapper>();

        _discountService = new DiscountService(
            _roomClassRepositoryMock.Object,
            _discountRepositoryMock.Object,
            _unitOfWorkMock.Object,
            _mapperMock.Object);
    }

    [Fact]
    public async Task ReturnDiscountWhenDiscountExists()
    {
        var roomClassId = Guid.NewGuid();
        var discountId = Guid.NewGuid();
        var discount = new Discount { Id = discountId, RoomClassId = roomClassId };
        var discountResponse = new DiscountResponse();

        _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
            .ReturnsAsync(true);

        _discountRepositoryMock.Setup(repo => repo.GetByIdAsync(roomClassId, discountId))
            .ReturnsAsync(discount);

        _mapperMock.Setup(mapper => mapper.Map<DiscountResponse>(discount))
            .Returns(discountResponse);

        var result = await _discountService.GetDiscountByIdAsync(roomClassId, discountId);

        Assert.NotNull(result);
        Assert.Equal(discountResponse, result);
    }

    [Fact]
    public async Task ThrowExceptionWhenRoomClassNotFound()
    {
        var roomClassId = Guid.NewGuid();
        var request = new DiscountCreationRequest();

        _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<RoomClassNotFoundException>(() => _discountService.CreateDiscountAsync(roomClassId, request));
    }

    [Fact]
    public async Task CreateDiscountWhenValid()
    {
        var roomClassId = Guid.NewGuid();
        var request = new DiscountCreationRequest { StartDate = DateTime.UtcNow, EndDate = DateTime.UtcNow.AddDays(5) };
        var discount = new Discount();
        var discountResponse = new DiscountResponse();

        _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
            .ReturnsAsync(true);
        _discountRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(false);
        _mapperMock.Setup(mapper => mapper.Map<Discount>(request))
            .Returns(discount);
        _discountRepositoryMock.Setup(repo => repo.CreateAsync(discount))
            .ReturnsAsync(discount);
        _mapperMock.Setup(mapper => mapper.Map<DiscountResponse>(discount))
            .Returns(discountResponse);

        var result = await _discountService.CreateDiscountAsync(roomClassId, request);

        Assert.NotNull(result);
        Assert.Equal(discountResponse, result);
    }

    [Fact]
    public async Task ThrowExceptionWhenRoomClassNotFoundWhileDeleting()
    {
        var roomClassId = Guid.NewGuid();
        var discountId = Guid.NewGuid();

        _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
            .ReturnsAsync(false);

        await Assert.ThrowsAsync<RoomClassNotFoundException>(() => _discountService.DeleteDiscountAsync(roomClassId, discountId));
    }

    [Fact]
    public async Task DeleteDiscountWhenValid()
    {
        var roomClassId = Guid.NewGuid();
        var discountId = Guid.NewGuid();

        _roomClassRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<RoomClass, bool>>>()))
            .ReturnsAsync(true);
        _discountRepositoryMock.Setup(repo => repo.ExistsAsync(It.IsAny<Expression<Func<Discount, bool>>>()))
            .ReturnsAsync(true);
        _discountRepositoryMock.Setup(repo => repo.DeleteAsync(discountId))
            .Returns(Task.CompletedTask);

        await _discountService.DeleteDiscountAsync(roomClassId, discountId);

        _discountRepositoryMock.Verify(repo => repo.DeleteAsync(discountId), Times.Once);
        _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
    }
}
