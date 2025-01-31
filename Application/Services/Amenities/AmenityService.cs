using Application.Contracts;
using Application.DTOs.Amenities;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Services.Amenities
{
    public class AmenityService : IAmenityService
    {
        private readonly IAmenityRepository _amenityRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public AmenityService(IAmenityRepository amenityRepository, IUnitOfWork unitOfWork, IMapper mapper)
        {
            _amenityRepository = amenityRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<AmenityResponse>> GetAmenitiesAsync(AmenitiesGetRequest request)
        {
            var query = new Query<Amenity>(
                GetSearchExpression(request.SearchTerm),
                request.SortOrder ?? SortMethod.Ascending,
                request.SortColumn,
                request.PageNumber,
                request.PageSize
            );

            var amenities = await _amenityRepository.GetAsync(query);
            var mappedItems = amenities.Items.Select(a => _mapper.Map<AmenityResponse>(a));
            return new PaginatedList<AmenityResponse>(mappedItems, amenities.PaginationMetadata);
        }

        public async Task<AmenityResponse> GetAmenityByIdAsync(Guid id)
        {
            var amenity = await _amenityRepository.GetByIdAsync(id)
                          ?? throw new Exception("Amenity not found.");
            return _mapper.Map<AmenityResponse>(amenity);
        }

        public async Task<AmenityResponse> CreateAmenityAsync(AmenityCreationRequest request)
        {
            if (await _amenityRepository.ExistAsync(a => a.Name == request.Name))
            {
                throw new Exception("Amenity with this name already exists.");
            }//may remove this check if we want to allow duplicate names

            var newAmenity = _mapper.Map<Amenity>(request);
            var createdAmenity = await _amenityRepository.CreateAsync(newAmenity);

            await _unitOfWork.SaveChangesAsync();
            return _mapper.Map<AmenityResponse>(createdAmenity);
        }

        public async Task UpdateAmenityAsync(Guid id, AmenityUpdateRequest request)
        {
            var amenityEntity = await _amenityRepository.GetByIdAsync(id)
                                 ?? throw new Exception("Amenity not found.");

            if (await _amenityRepository.ExistAsync(a => a.Name == request.Name && a.Id != id))
            {
                throw new Exception("Another amenity with this name already exists.");
            }

            _mapper.Map(request, amenityEntity);
            await _amenityRepository.UpdateAsync(amenityEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<Amenity, bool>> GetSearchExpression(string? searchTerm)
        {
            return string.IsNullOrEmpty(searchTerm)
                ? _ => true
                : a => a.Name.Contains(searchTerm);
        }
    }
}
