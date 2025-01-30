using Application.DTOs.Amenities;

namespace Application.Contracts
{
    public interface IAmenityService
    {
        Task<IEnumerable<AmenityResponse>> GetAmenitiesAsync(AmenitiesGetRequest request);
        Task<AmenityResponse> GetAmenityByIdAsync(Guid id);
        Task<AmenityResponse> CreateAmenityAsync(AmenityCreationRequest request);
        Task UpdateAmenityAsync(Guid id, AmenityUpdateRequest request);
    }
}
