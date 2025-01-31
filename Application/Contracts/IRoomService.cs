using Application.DTOs.Rooms;
using Domain.Models;


namespace Application.Contracts
{
    public interface IRoomService
    {
        Task<PaginatedList<RoomForManagementResponse>> GetRoomsForManagementAsync(Guid roomClassId, RoomsGetRequest request);
        Task<PaginatedList<RoomForGuestResponse>> GetRoomsForGuestsAsync(Guid roomClassId, RoomsForGuestsGetRequest request);
        Task<Guid> CreateRoomAsync(Guid roomClassId, RoomCreationRequest request);
        Task UpdateRoomAsync(Guid roomClassId, Guid id, RoomUpdateRequest request);
        Task DeleteRoomAsync(Guid roomClassId, Guid id);
    }
}
