using Application.Cities.Models;
using Application.Contracts;
using Application.DTOs.Cities;
using Application.DTOs.Images;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Cities
{
    public class CityService : ICityService
    {
        private readonly ICityRepository _cityRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public CityService(
            ICityRepository cityRepository,
            IHotelRepository hotelRepository,
            IImageRepository imageRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _cityRepository = cityRepository;
            _hotelRepository = hotelRepository;
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<CityForManagementResponse>> GetCitiesForManagementAsync(
            CitiesGetHandler request)
        {
            var query = new Query<City>(
                GetSearchExpression(request.SearchTerm),
                request.SortOrder ?? SortMethod.Ascending,
                request.SortColumn,
                request.PageNumber,
                request.PageSize);

            var cities = await _cityRepository.GetForManagement(query);
            var mappedItems = cities.Items
                .Select(city => _mapper.Map<CityForManagementResponse>(city)).ToList();

            return new PaginatedList<CityForManagementResponse>(mappedItems, cities.PaginationMetadata);
        }

        public async Task<IEnumerable<TrendingCityResponse>> GetTrendingCitiesAsync(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentException("Count must be greater than zero.");
            }

            var cities = await _cityRepository.GetMostVisitedAsync(count);
            return _mapper.Map<IEnumerable<TrendingCityResponse>>(cities);
        }

        public async Task CreateCityAsync(CityCreationRequest request)
        {
            if (await _cityRepository.ExistsAsync(c => c.PostOffice == request.PostOffice))
            {
                throw new Exception("City with post office exists");
            }
            var city = _mapper.Map<City>(request);
            await _cityRepository.CreateAsync(city);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCityAsync(Guid id, CityUpdateRequest request)
        {
            if (await _cityRepository.ExistsAsync(c => c.PostOffice == request.PostOffice))
            {
                throw new Exception("City With Post Office Exists");
            }

            var city = await _cityRepository.GetByIdAsync(id) ??
                       throw new Exception($"Not found {id}");

            _mapper.Map(request, city);
            await _cityRepository.UpdateAsync(city);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCityAsync(Guid id)
        {
            if (!await _cityRepository.ExistsAsync(c => c.Id == id))
            {
                throw new Exception($"Not Found {id}");
            }

            if (await _hotelRepository.ExistsAsync(h => h.CityId == id))
            {
                throw new Exception($"DependentsExist on this ID : {id}");
            }

            await _cityRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SetCityThumbnailAsync(Guid id, ImageCreationRequest request)
        {
            if (!await _cityRepository.ExistsAsync(c => c.Id == id))
            {
                throw new Exception($"Not Found {id}");
            }

            await _imageRepository.DeleteAsync(id, ImageType.Thumbnail);
            await _imageRepository.CreateAsync(request.Image, id, ImageType.Thumbnail);
            await _unitOfWork.SaveChangesAsync();
        }

        private static Expression<Func<City, bool>> GetSearchExpression(string? searchTerm)
        {
            return searchTerm is null
                ? _ => true
                : c => c.Name.Contains(searchTerm) || c.Country.Contains(searchTerm);
        }
    }
}
