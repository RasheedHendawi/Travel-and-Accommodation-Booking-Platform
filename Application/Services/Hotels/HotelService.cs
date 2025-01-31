using Application.Contracts;
using Application.DTOs.Hotels;
using Application.DTOs.Images;
using Application.Helper;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using Microsoft.AspNetCore.Http;
using System.Linq.Expressions;

namespace Application.Services.Hotels
{
    public class HotelsService : IHotelService
    {
        private readonly IHotelRepository _hotelRepository;
        private readonly ICityRepository _cityRepository;
        private readonly IOwnerRepository _ownerRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public HotelsService(
            IHotelRepository hotelRepository,
            ICityRepository cityRepository,
            IOwnerRepository ownerRepository,
            IImageRepository imageRepository,
            IRoomClassRepository roomClassRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _hotelRepository = hotelRepository;
            _cityRepository = cityRepository;
            _ownerRepository = ownerRepository;
            _imageRepository = imageRepository;
            _roomClassRepository = roomClassRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<HotelGetFromManagment>> GetHotelsForManagementAsync(HotelGetRequest request)
        {
            var query = new Query<Hotel>(
                h => string.IsNullOrEmpty(request.SearchTerm) ||
                     h.Name.Contains(request.SearchTerm) || h.City.Name.Contains(request.SearchTerm),
                request.SortOrder ?? SortMethod.Ascending,
                request.SortColumn,
                request.PageNumber,
                request.PageSize
            );

            var hotels = await _hotelRepository.GetForManagementAsync(query);
            var mappedItems = hotels.Items
                .Select(hotel => _mapper.Map<HotelGetFromManagment>(hotel))
                .ToList();

            return new PaginatedList<HotelGetFromManagment>(mappedItems, hotels.PaginationMetadata);

        }

        public async Task<HotelGetResponse> GetHotelByIdAsync(Guid id)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id, true, true, true)
                        ?? throw new Exception("Hotel not found.");
            return _mapper.Map<HotelGetResponse>(hotel);
        }

        public async Task<Guid> CreateHotelAsync(HotelCreationRequest request)
        {
            if (!await _cityRepository.ExistsAsync(c => c.Id == request.CityId))
                throw new Exception("City not found.");

            if (!await _ownerRepository.ExistsAsync(o => o.Id == request.OwnerId))
                throw new Exception("Owner not found.");

            if (await _hotelRepository.ExistsAsync(h => h.Longitude == request.Longitude && h.Latitude == request.Latitude))
                throw new Exception("A hotel at this location already exists.");

            var hotel = _mapper.Map<Hotel>(request);
            var createdHotel = await _hotelRepository.CreateAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
            return createdHotel.Id;
        }

        public async Task UpdateHotelAsync(Guid id, HotelUpdateRequest request)
        {
            var hotel = await _hotelRepository.GetByIdAsync(id)
                        ?? throw new Exception("Hotel not found.");

            _mapper.Map(request, hotel);
            await _hotelRepository.UpdateAsync(hotel);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteHotelAsync(Guid id)
        {
            if (!await _hotelRepository.ExistsAsync(h => h.Id == id))
                throw new Exception("Hotel not found.");

            if (await _roomClassRepository.ExistsAsync(rc => rc.HotelId == id))
                throw new Exception("Cannot delete hotel with existing room classes.");

            await _hotelRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task SetHotelThumbnailAsync(Guid id, ImageCreationRequest request)
        {
            if (!await _hotelRepository.ExistsAsync(h => h.Id == id))
                throw new Exception("Hotel not found.");

            await _imageRepository.DeleteAsync(id, ImageType.Thumbnail);
            await _imageRepository.CreateAsync(request.Image, id, ImageType.Thumbnail);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddImageToGalleryAsync(Guid id, ImageCreationRequest request)
        {
            if (!await _hotelRepository.ExistsAsync(h => h.Id == id))
                throw new Exception("Hotel not found.");

            await _imageRepository.CreateAsync(request.Image, id, ImageType.Gallery);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<IEnumerable<HotelFeaturedDealResponse>> GetFeaturedDealsAsync(int count)
        {
            if (count <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(count), "Count must be greater than zero.");
            }

            var featuredDeals = await _roomClassRepository.GetFeaturedDealsAsync(count);
            return featuredDeals.Select(deal => _mapper.Map<HotelFeaturedDealResponse>(deal)).ToList();
        }

        public async Task<PaginatedList<HotelSearchResponse>> SearchAndFilterHotelsAsync(HotelSearchRequest request)
        {
            var searchResults = await _hotelRepository.GetForSearchAsync(
                new Query<Hotel>(
                    BuildSearchExpression(request),
                    request.SortOrder ?? SortMethod.Ascending,
                    request.SortColumn,
                    request.PageNumber,
                    request.PageSize));

            var mappedItems = searchResults.Items
                .Select(hotel => _mapper.Map<HotelSearchResponse>(hotel))
                .ToList();

            return new PaginatedList<HotelSearchResponse>(mappedItems, searchResults.PaginationMetadata);
        }

        private static Expression<Func<Hotel, bool>> BuildSearchExpression(HotelSearchRequest request)
        {
            return CreateSearchTermExpression(request)
                .And(CreateRoomTypeExpression(request))
                .And(CreateCapacityExpression(request))
                .And(CreatePriceRangeExpression(request))
                .And(CreateAmenitiesExpression(request))
                .And(CreateAvailableRoomsExpression(request))
                .And(CreateMinStarRatingExpression(request));
        }

        private static Expression<Func<Hotel, bool>> CreateSearchTermExpression(HotelSearchRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                return _ => true;
            }

            return h => h.Name.Contains(request.SearchTerm) ||
                        h.City.Name.Contains(request.SearchTerm) ||
                        h.City.Country.Contains(request.SearchTerm);
        }

        private static Expression<Func<Hotel, bool>> CreateRoomTypeExpression(HotelSearchRequest request)
        {
            if (request.RoomTypes.Any())
            {
                return h => h.RoomClasses.Any(rc => request.RoomTypes.Contains(rc.RoomType));
            }

            return _ => true;
        }

        private static Expression<Func<Hotel, bool>> CreateCapacityExpression(HotelSearchRequest request)
        {
            return h => h.RoomClasses.Any(rc =>
                rc.AdultCapacity >= request.NumberOfAdults && rc.ChildCapacity >= request.NumberOfChildren);
        }

        private static Expression<Func<Hotel, bool>> CreatePriceRangeExpression(HotelSearchRequest request)
        {
            Expression<Func<Hotel, bool>> greaterThanMinPriceExpression =
                request.MinPrice.HasValue
                    ? h => h.RoomClasses.Any(rc => rc.Price >= request.MinPrice)
                    : _ => true;

            Expression<Func<Hotel, bool>> lessThanMaxPriceExpression =
                request.MaxPrice.HasValue
                    ? h => h.RoomClasses.Any(rc => rc.Price <= request.MaxPrice)
                    : _ => true;

            return greaterThanMinPriceExpression
                .And(lessThanMaxPriceExpression);
        }

        private static Expression<Func<Hotel, bool>> CreateAmenitiesExpression(HotelSearchRequest request)
        {
            if (request.Amenities.Any())
            {
                return h => request.Amenities.All(amenityId => h.RoomClasses.Any(rc => rc.Amenities.Any(a => a.Id == amenityId)));
            }

            return _ => true;
        }

        private static Expression<Func<Hotel, bool>> CreateAvailableRoomsExpression(HotelSearchRequest request)
        {
            return h => h.RoomClasses.Any(rc => rc.Rooms.Count(r =>
                !r.Bookings.Any(b => request.CheckOutDate <= b.CheckIn || request.CheckInDate >= b.CheckOut)
            ) >= request.NumberOfRooms);
        }

        private static Expression<Func<Hotel, bool>> CreateMinStarRatingExpression(HotelSearchRequest request)
        {
            if (request.MinStarRating.HasValue)
            {
                return h => h.StartRating >= request.MinStarRating;
            }

            return _ => true;
        }

    }
}
