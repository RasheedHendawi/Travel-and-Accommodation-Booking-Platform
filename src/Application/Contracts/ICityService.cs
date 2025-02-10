using Application.DTOs.Cities;
using Application.DTOs.Images;
using Domain.Models;

namespace Application.Contracts
{
    public interface ICityService
    {
        Task<PaginatedList<CityForManagementResponse>> GetCitiesForManagementAsync(CitiesGetHandler request);
        Task<IEnumerable<TrendingCityResponse>> GetTrendingCitiesAsync(TrendingCityRequest request);
        Task CreateCityAsync(CityCreationRequest request);
        Task UpdateCityAsync(Guid id, CityUpdateRequest request);
        Task DeleteCityAsync(Guid id);
        Task SetCityThumbnailAsync(Guid id, ImageCreationRequest request);
    }
}
