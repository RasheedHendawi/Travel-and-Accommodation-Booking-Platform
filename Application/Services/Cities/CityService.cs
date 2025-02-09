using Application.Contracts;
using Application.DTOs.Cities;
using Application.DTOs.Images;
using Application.Exceptions.GeneralExceptions;
using Application.Exceptions.HotelExceptions;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Services.Cities
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

        public async Task<IEnumerable<TrendingCityResponse>> GetTrendingCitiesAsync(TrendingCityRequest request)
        {
            if (request.Count <= 0)
            {
                throw new ArgumentOutOfRangeException(request.Count.ToString());
            }

            var cities = await _cityRepository.GetMostVisitedAsync(request.Count);
            return _mapper.Map<IEnumerable<TrendingCityResponse>>(cities);
        }

        public async Task CreateCityAsync(CityCreationRequest request)
        {
            if (await _cityRepository.ExistsAsync(c => c.PostOffice == request.PostOffice))
            {
                throw new CityWithPostofficeException();
            }
            var city = _mapper.Map<City>(request);
            await _cityRepository.CreateAsync(city);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task UpdateCityAsync(Guid id, CityUpdateRequest request)
        {
            if (await _cityRepository.ExistsAsync(c => c.PostOffice == request.PostOffice))
            {
                throw new CityWithPostofficeException();
            }

            var city = await _cityRepository.GetByIdAsync(id) ??
                       throw new CityNotFoundException();

            _mapper.Map(request, city);
            await _cityRepository.UpdateAsync(city);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteCityAsync(Guid id)
        {
            if (!await _cityRepository.ExistsAsync(c => c.Id == id))
            {
                throw new CityNotFoundException();
            }

            if (await _hotelRepository.ExistsAsync(h => h.CityId == id))
            {
                throw new DependencyDeletionException("City", "Hotel");
            }

            await _cityRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SetCityThumbnailAsync(Guid id, ImageCreationRequest request)
        {
            if (!await _cityRepository.ExistsAsync(c => c.Id == id))
            {
                throw new CityNotFoundException();
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
