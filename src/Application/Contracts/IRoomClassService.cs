

using Application.DTOs.Images;
using Application.DTOs.RoomClass;
using Domain.Entities;
using Domain.Models;

namespace Application.Contracts
{
    public interface IRoomClassService
    {
        Task<PaginatedList<RoomClassForManagementResponse>> GetAllRoomClassesAsync(ResourcesQueryRequest resourcesQueryRequest);
        Task<RoomClassForManagementResponse> GetRoomClassByIdAsync(Guid id);
        Task<Guid> CreateRoomClassAsync(RoomClassCreationRequest roomClass);
        Task UpdateRoomClassAsync(Guid id, RoomClassUpdateRequest updatedRoomClass);
        Task DeleteRoomClassAsync(Guid id);
        Task AddImageToRoomClassAsync(Guid roomClassId, ImageCreationRequest image);
    }
}
