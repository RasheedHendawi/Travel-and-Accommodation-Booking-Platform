using Application.DTOs.Amenities;
using Application.DTOs.Shared;
using Application.Services.Amenities;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Moq;
using System.Linq.Expressions;


namespace Application.Tests.Services.Amenities
{
    public class AmenitiesServiceShould
    {
        private readonly Mock<IAmenityRepository> _amenityRepositoryMock;
        private readonly Mock<IUnitOfWork> _unitOfWorkMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly AmenityService _amenityService;

        public AmenitiesServiceShould()
        {
            _amenityRepositoryMock = new Mock<IAmenityRepository>();
            _unitOfWorkMock = new Mock<IUnitOfWork>();
            _mapperMock = new Mock<IMapper>();

            _amenityService = new AmenityService(
                _amenityRepositoryMock.Object,
                _unitOfWorkMock.Object,
                _mapperMock.Object);
        }

        [Fact]
        public async Task ReturnAmenityWhenAmenityExists()
        {

            var amenityId = Guid.NewGuid();
            var amenity = new Amenity { Id = amenityId, Name = "WiFi" };
            var amenityResponse = new AmenityResponse { Id = amenityId, Name = "WiFi" };

            _amenityRepositoryMock.Setup(repo => repo.GetByIdAsync(amenityId))
                .ReturnsAsync(amenity);

            _mapperMock.Setup(mapper => mapper.Map<AmenityResponse>(amenity))
                .Returns(amenityResponse);


            var result = await _amenityService.GetAmenityByIdAsync(amenityId);

            Assert.NotNull(result);
            Assert.Equal(amenityId, result.Id);
            Assert.Equal("WiFi", result.Name);
        }

        [Fact]
        public async Task ThrowExceptionWhenAmenityDoesNotExist()
        {
            var amenityId = Guid.NewGuid();
            _amenityRepositoryMock.Setup(repo => repo.GetByIdAsync(amenityId))
                .ReturnsAsync(null as Amenity);

            await Assert.ThrowsAsync<Exception>(() => _amenityService.GetAmenityByIdAsync(amenityId));
        }

        [Fact]
        public async Task CreateAndReturnAmenityWhenValidDataProvided()
        {
            var creationRequest = new AmenityCreationRequest { Name = "Wifi" };
            var newAmenity = new Amenity { Id = Guid.NewGuid(), Name = "Wifi" };
            var amenityResponse = new AmenityResponse { Id = newAmenity.Id, Name = "Wifi" };

            _amenityRepositoryMock.Setup(repo => repo.ExistAsync(It.IsAny<Expression<Func<Amenity, bool>>>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(mapper => mapper.Map<Amenity>(creationRequest))
                .Returns(newAmenity);

            _amenityRepositoryMock.Setup(repo => repo.CreateAsync(newAmenity))
                .ReturnsAsync(newAmenity);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(1);

            _mapperMock.Setup(mapper => mapper.Map<AmenityResponse>(newAmenity))
                .Returns(amenityResponse);

            var result = await _amenityService.CreateAmenityAsync(creationRequest);

            Assert.NotNull(result);
            Assert.Equal("Wifi", result.Name);
        }

        [Fact]
        public async Task ThrowExceptionWhenDuplicateNameExistsInCreateAsync()
        {
            var creationRequest = new AmenityCreationRequest { Name = "Wifi" };

            _amenityRepositoryMock.Setup(repo => repo.ExistAsync(It.IsAny<Expression<Func<Amenity, bool>>>()))
                .ReturnsAsync(true);

            await Assert.ThrowsAsync<Exception>(() => _amenityService.CreateAmenityAsync(creationRequest));
        }

        [Fact]
        public async Task UpdateAmenityWhenValidDataProvided()
        {
            var amenityId = Guid.NewGuid();
            var updateRequest = new AmenityUpdateRequest { Name = "Updated Wifi" };
            var existingAmenity = new Amenity { Id = amenityId, Name = "Wifi" };

            _amenityRepositoryMock.Setup(repo => repo.GetByIdAsync(amenityId))
                .ReturnsAsync(existingAmenity);

            _amenityRepositoryMock.Setup(repo => repo.ExistAsync(It.IsAny<Expression<Func<Amenity, bool>>>()))
                .ReturnsAsync(false);

            _mapperMock.Setup(mapper => mapper.Map(updateRequest, existingAmenity));

            _amenityRepositoryMock.Setup(repo => repo.UpdateAsync(existingAmenity))
                .Returns(Task.CompletedTask);

            _unitOfWorkMock.Setup(uow => uow.SaveChangesAsync())
                .ReturnsAsync(1);


            await _amenityService.UpdateAmenityAsync(amenityId, updateRequest);

            _amenityRepositoryMock.Verify(repo => repo.UpdateAsync(existingAmenity), Times.Once);
            _unitOfWorkMock.Verify(uow => uow.SaveChangesAsync(), Times.Once);
        }

        [Fact]
        public async Task ThrowExceptionWhenUpdatingAnAmenityAndAmenityDoesNotExist()
        {
            var amenityId = Guid.NewGuid();
            var updateRequest = new AmenityUpdateRequest { Name = "Updated Gym" };

            _amenityRepositoryMock.Setup(repo => repo.GetByIdAsync(amenityId))
                .ReturnsAsync(null as Amenity);
                
            await Assert.ThrowsAsync<Exception>(() => _amenityService.UpdateAmenityAsync(amenityId, updateRequest));
        }
        [Fact]
        public async Task ReturnPaginatedListWhenValidRequest()
        {
            var request = new AmenitiesGetRequest
            {
                SearchTerm = "Wifi",
                SortOrder = SortMethod.Ascending,
                SortColumn = "Name",
                PageNumber = 1,
                PageSize = 10
            };

            var amenities = new List<Amenity>
            {
                new() { Id = Guid.NewGuid(), Name = "Wifi" },
                new() { Id = Guid.NewGuid(), Name = "Gym" }
            };

            var paginatedAmenities = new PaginatedList<Amenity>(amenities, new PaginationMetaData(1, 2, 10));

            _amenityRepositoryMock.Setup(repo => repo.GetAsync(It.IsAny<Query<Amenity>>()))
                .ReturnsAsync(paginatedAmenities);

            _mapperMock.Setup(mapper => mapper.Map<AmenityResponse>(It.IsAny<Amenity>()))
                .Returns((Amenity amenity) => new AmenityResponse { Id = amenity.Id, Name = amenity.Name });


            var result = await _amenityService.GetAmenitiesAsync(request);

            Assert.NotNull(result);
            Assert.Equal(2, result.Items.Count());
            Assert.Contains(result.Items, a => a.Name == "Wifi");
            Assert.Contains(result.Items, a => a.Name == "Gym");
        }
        [Fact]
        public async Task ThrowExceptionWhenDuplicateNameExistsInUpdateAsync()
        {
            var amenityId = Guid.NewGuid();
            var updateRequest = new AmenityUpdateRequest { Name = "Wifi" };
            var existingAmenity = new Amenity { Id = amenityId, Name = "Old Name" };

            _amenityRepositoryMock.Setup(repo => repo.GetByIdAsync(amenityId))
                .ReturnsAsync(existingAmenity);

            _amenityRepositoryMock.Setup(repo => repo.ExistAsync(It.IsAny<Expression<Func<Amenity, bool>>>()))
                .ReturnsAsync(true); 

            
            await Assert.ThrowsAsync<Exception>(() => _amenityService.UpdateAmenityAsync(amenityId, updateRequest));
        }

    }
}
