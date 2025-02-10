using Application.DTOs.Hotels;
using Application.DTOs.Images;
using Domain.Models;

namespace Application.Contracts
{
    public interface IHotelService
    {
        Task<IEnumerable<HotelFeaturedDealResponse>> GetFeaturedDealsAsync(FeaturedDealsRequest request);
        Task<PaginatedList<HotelSearchResponse>> SearchAndFilterHotelsAsync(HotelSearchRequest request);
        Task SetHotelThumbnailAsync(Guid id, ImageCreationRequest image);
        Task AddImageToGalleryAsync(Guid id, ImageCreationRequest image);
        Task DeleteHotelAsync(Guid id);
        Task UpdateHotelAsync(Guid id, HotelUpdateRequest request);
        Task<Guid> CreateHotelAsync(HotelCreationRequest request);
        Task<PaginatedList<HotelGetFromManagment>> GetHotelsForManagementAsync(HotelGetRequest request);
        Task<HotelGetResponse> GetHotelByIdAsync(Guid id);
    }
}
