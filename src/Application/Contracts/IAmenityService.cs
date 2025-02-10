using Application.DTOs.Amenities;
using Application.DTOs.Shared;
using Domain.Models;

namespace Application.Contracts
{
    public interface IAmenityService
    {
        Task<PaginatedList<AmenityResponse>> GetAmenitiesAsync(AmenitiesGetRequest request);
        Task<AmenityResponse> GetAmenityByIdAsync(Guid id);
        Task<AmenityResponse> CreateAmenityAsync(AmenityCreationRequest request);
        Task UpdateAmenityAsync(Guid id, AmenityUpdateRequest request);
    }
}
