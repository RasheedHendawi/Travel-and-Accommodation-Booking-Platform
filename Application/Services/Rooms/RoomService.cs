using Application.Contracts;
using Application.DTOs.Rooms;
using AutoMapper;
using Domain.Entities;
using Domain.Enums;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.UnitOfWork;
using Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Rooms
{
    public class RoomService : IRoomService
    {
        private readonly IRoomRepository _roomRepository;
        private readonly IBookingRepository _bookingRepository;
        private readonly IRoomClassRepository _roomClassRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public RoomService(
            IRoomRepository roomRepository,
            IBookingRepository bookingRepository,
            IRoomClassRepository roomClassRepository,
            IUnitOfWork unitOfWork,
            IMapper mapper)
        {
            _roomRepository = roomRepository;
            _bookingRepository = bookingRepository;
            _roomClassRepository = roomClassRepository;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public async Task<PaginatedList<RoomForManagementResponse>> GetRoomsForManagementAsync(
            Guid roomClassId,
            RoomsGetRequest request)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            var query = new Query<Room>(
                r => r.RoomClassId == roomClassId,
                request.SortOrder ?? SortMethod.Ascending,
                request.SortColumn,
                request.PageNumber,
                request.PageSize);

            var rooms = await _roomRepository.GetForManagementAsync(query);
            return _mapper.Map<PaginatedList<RoomForManagementResponse>>(rooms);
        }

        public async Task<PaginatedList<RoomForGuestResponse>> GetRoomsForGuestsAsync(
            Guid roomClassId,
            RoomsForGuestsGetRequest request)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            var query = new Query<Room>(
                r => r.RoomClassId == roomClassId &&
                     !r.Bookings.Any(b => request.CheckIn >= b.CheckOut
                                          || request.CheckOut <= b.CheckIn),
                SortMethod.Ascending,
                null,
                request.PageNumber,
                request.PageSize);

            var rooms = await _roomRepository.GetAsync(query);
            return _mapper.Map<PaginatedList<RoomForGuestResponse>>(rooms);
        }

        public async Task<Guid> CreateRoomAsync(Guid roomClassId, RoomCreationRequest request)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            if (await _roomRepository.ExistsAsync(r => r.RoomClassId == roomClassId && r.Number == request.Number))
            {
                throw new Exception("RoomClass Duplicated Room Number");
            }
            var roomHandler = new CreateRoomHandler() { RoomClassId = roomClassId };
            _mapper.Map(request, roomHandler);
            var createdRoom = _mapper.Map<Room>(roomHandler);
            createdRoom.CreatedAt = DateTime.UtcNow;
            await _roomRepository.CreateAsync(createdRoom);
            await _unitOfWork.SaveChangesAsync();
            return createdRoom.Id;
        }

        public async Task UpdateRoomAsync(Guid roomClassId, Guid id, RoomUpdateRequest request)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }
            if (await _roomRepository.ExistsAsync(
                   r => r.RoomClassId == roomClassId &&
                        r.Number == request.Number))
            {
                throw new Exception("Room With Number Exists In RoomClass");
            }
            var roomEntity = await _roomRepository.GetByIdAsync(roomClassId, id) ??
                throw new Exception("Room Not Found");

            var updatehandler = new UpdateRoomHandler
            {
                RoomClassId = roomClassId,
                RoomId = id,
                Number = request.Number
            };
            _mapper.Map(updatehandler, roomEntity);
            roomEntity.UpdatedAt = DateTime.UtcNow;
            await _roomRepository.UpdateAsync(roomEntity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task DeleteRoomAsync(Guid roomClassId, Guid id)
        {
            if (!await _roomClassRepository.ExistsAsync(rc => rc.Id == roomClassId))
            {
                throw new Exception("RoomClass Not Found");
            }

            if (!await _roomRepository.ExistsAsync(r => r.Id == id && r.RoomClassId == roomClassId))
            {
                throw new Exception("Room Not Found");
            }
            if (await _bookingRepository.ExistsAsync(b => b.Rooms.Any(r => r.Id == id)))
            {
                throw new Exception("Dependents Exist");
            }
            await _roomRepository.DeleteAsync(id);
            await _unitOfWork.SaveChangesAsync();
        }
    }
}

