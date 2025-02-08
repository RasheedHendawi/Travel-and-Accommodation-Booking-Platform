using Application.Contracts;
using Application.DTOs.Images;
using Application.DTOs.RoomClass;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System.Linq.Expressions;

namespace Application.Services.RoomClasses
{
    public class RoomClassService : IRoomClassService
    {
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IHotelRepository _hotelRepository;
        private readonly IAmenityRepository _amenityRepository;
        private readonly IImageRepository _imageRepository;
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;

        public RoomClassService(
            IRoomClassRepository roomClassRepository,
            IHotelRepository hotelRepository,
            IAmenityRepository amenityRepository,
            IImageRepository imageRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _roomClassRepository = roomClassRepository;
            _hotelRepository = hotelRepository;
            _amenityRepository = amenityRepository;
            _imageRepository = imageRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RoomClassForManagementResponse>> GetAllRoomClassesAsync(
             ResourcesQueryRequest request)
        {
            var query = new Query<RoomClass>(
              GetSearchExpression(request.SearchTerm),
              request.SortOrder ?? SortMethod.Ascending,
              request.SortColumn,
              request.PageNumber,
              request.PageSize);
            var owners = await _roomClassRepository.GetAsync(query, false);
            return _mapper.Map<PaginatedList<RoomClassForManagementResponse>>(owners);

        }
        private static Expression<Func<RoomClass, bool>> GetSearchExpression(string? searchTerm)
        {
            return searchTerm is null
              ? _ => true
              : rc => rc.Name.Contains(searchTerm);
        }
        public async Task<RoomClassForManagementResponse> GetRoomClassByIdAsync(Guid id)
        {
            var roomClass = await _roomClassRepository.GetByIdAsync(id);
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == id))
            {
                throw new Exception("RoomClass Not Found");
            }
            var tmpRoom = _mapper.Map<RoomClassForManagementResponse>(roomClass);
            return tmpRoom;
        }


        public async Task<Guid> CreateRoomClassAsync(RoomClassCreationRequest roomClass)
        {
            if (!await _hotelRepository.ExistsAsync(h => h.Id == roomClass.HotelId))
            {
                throw new Exception("Hotel Not Found");
            }

            if (await _roomClassRepository.ExistsAsync(rc => rc.HotelId == roomClass.HotelId && rc.Name == roomClass.Name))
            {
                throw new Exception("Name with Hotel Found");
            }

            foreach (var amenityId in roomClass.AmenitiesId)
            {
                if (!await _amenityRepository.ExistAsync(a => a.Id == amenityId))
                {
                    throw new Exception("Amenity With Id Not Found");
                }
            }

            var roomClassObj = _mapper.Map<RoomClass>(roomClass);
            foreach (var amenitieId in roomClass.AmenitiesId)
            {
                roomClassObj.Amenities.Add(await _amenityRepository.GetByIdAsync(amenitieId));
            }
            var createdRoomClass = await _roomClassRepository.CreateAsync(roomClassObj);
            await _unitOfWork.SaveChangesAsync();
            return createdRoomClass.Id;
        }

        public async Task UpdateRoomClassAsync(Guid id, RoomClassUpdateRequest updatedRoomClass)
        {
            var existingRoomClass = await _roomClassRepository.GetByIdAsync(id);
            if (existingRoomClass == null)
            {
                throw new Exception("RoomClass Not Found");
            }

            if (await _roomClassRepository.ExistsAsync(rc => rc.HotelId == existingRoomClass.HotelId && rc.Name == updatedRoomClass.Name && rc.Id != id))
            {
                throw new Exception("RoomClass Name With Hotel Found");
            }

            _mapper.Map(updatedRoomClass, existingRoomClass);
            await _roomClassRepository.UpdateAsync(existingRoomClass);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoomClassAsync(Guid id)
        {
            var roomClass = await _roomClassRepository.GetByIdAsync(id);
            if (roomClass == null)
            {
                throw new Exception("RoomClass Not Found");
            }

            await _roomClassRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task AddImageToRoomClassAsync(Guid roomClassId, ImageCreationRequest image)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }
            await _imageRepository.CreateAsync(image.Image, roomClassId, ImageType.Gallery);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}
