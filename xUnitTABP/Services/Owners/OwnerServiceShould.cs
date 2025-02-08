using Application.Contracts;
using Application.DTOs.Owners;
using Application.DTOs.Shared;
using Application.Services.Owners;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Moq;

namespace Application.Tests.Services.Owners
{
    public class OwnerServiceShould
    {
        private readonly Mock<IOwnerRepository> _ownerRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly OwnerService _ownerService;

        public OwnerServiceShould()
        {
            _ownerRepositoryMock = new Mock<IOwnerRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();
            _ownerService = new OwnerService(_ownerRepositoryMock.Object, _unitOfWorkMock.Object, _mapperMock.Object);
        }

        [Fact]
        public async Task ReturnPaginatedListWhenOwnersExist()
        {
            var request = new OwnersGetRequest { PageNumber = 1, PageSize = 2, SearchTerm = "Name", SortOrder = SortMethod.Ascending };
            var owners = new List<Owner> { new() { Id = Guid.NewGuid(), FirstName = "rasheed", LastName = "hendawi" } };
            var paginatedOwners = new PaginatedList<Owner>(owners, new PaginationMetaData(1, 1, 2));

            _ownerRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Query<Owner>>())).ReturnsAsync(paginatedOwners);
            _mapperMock.Setup(mapper => mapper.Map<PaginatedList<OwnersResponse>>(paginatedOwners)).Returns(new PaginatedList<OwnersResponse>(new List<OwnersResponse>(), new PaginationMetaData(1, 1, 2)));

            var result = await _ownerService.GetOwnersAsync(request);

            Assert.NotNull(result);
            Assert.Empty(result.Items);
        }

        [Fact]
        public async Task ReturnOwnerWhenOwnerExists()
        {
            var ownerId = Guid.NewGuid();
            var owner = new Owner { Id = ownerId, FirstName = "testing", LastName = "testing123" };
            var ownerResponse = new OwnersResponse { Id = ownerId };

            _ownerRepositoryMock.Setup(repo => repo.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            _mapperMock.Setup(mapper => mapper.Map<OwnersResponse>(owner)).Returns(ownerResponse);

            var result = await _ownerService.GetOwnerByIdAsync(ownerId);

            Assert.NotNull(result);
            Assert.Equal(ownerId, result.Id);
        }

        [Fact]
        public async Task ThrowExceptionWhenOwnerDoesNotExist()
        {

            var ownerId = Guid.NewGuid();
            _ownerRepositoryMock.Setup(repo => repo.GetByIdAsync(ownerId)).ReturnsAsync(null as Owner);

            await Assert.ThrowsAsync<Exception>(async () => await _ownerService.GetOwnerByIdAsync(ownerId));
        }

        [Fact]
        public async Task ShouldReturnCreatedOwner()
        {
            var request = new OwnerCreationRequest { FirstName = "test", LastName = "test123" };
            var owner = new Owner { Id = Guid.NewGuid(), FirstName = "test2", LastName = "test123456" };
            var ownerResponse = new OwnersResponse { Id = owner.Id };

            _mapperMock.Setup(mapper => mapper.Map<Owner>(request)).Returns(owner);
            _ownerRepositoryMock.Setup(repo => repo.CreateAsync(owner)).ReturnsAsync(owner);
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);
            _mapperMock.Setup(mapper => mapper.Map<OwnersResponse>(owner)).Returns(ownerResponse);

            var result = await _ownerService.CreateOwnerAsync(request);

            Assert.NotNull(result);
            Assert.Equal(owner.Id, result.Id);
        }

        [Fact]
        public async Task UpdateOwnerWhenOwnerExists()
        {
            var ownerId = Guid.NewGuid();
            var request = new OwnerUpdateRequest { FirstName = "Updated", LastName = "Name" };
            var owner = new Owner { Id = ownerId, FirstName = "Old", LastName = "Name" };

            _ownerRepositoryMock.Setup(repo => repo.GetByIdAsync(ownerId)).ReturnsAsync(owner);
            _mapperMock.Setup(mapper => mapper.Map(request, owner));
            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync()).ReturnsAsync(1);

            await _ownerService.UpdateOwnerAsync(ownerId, request);

            _ownerRepositoryMock.Verify(repo => repo.UpdateAsync(owner), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionWhenOwnerDoesNotExistWhenUpdating()
        {
            var ownerId = Guid.NewGuid();
            var request = new OwnerUpdateRequest { FirstName = "New1", LastName = "Name2" };

            _ownerRepositoryMock.Setup(repo => repo.GetByIdAsync(ownerId)).ReturnsAsync(null as Owner);

            await Assert.ThrowsAsync<Exception>(async () => await _ownerService.UpdateOwnerAsync(ownerId, request));
        }
    }
}
