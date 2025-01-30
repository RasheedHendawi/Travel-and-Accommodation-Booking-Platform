
using Application.DTOs.Owners;
using Domain.Models;

namespace Application.Contracts
{
    public interface IOwnerService
    {
        Task<PaginatedList<OwnersResponse>> GetOwnersAsync(OwnersGetRequest request);
        Task<OwnersResponse> GetOwnerByIdAsync(Guid id);
        Task<OwnersResponse> CreateOwnerAsync(OwnerCreationRequest request);
        Task UpdateOwnerAsync(Guid id, OwnerUpdateRequest request);
    }
}
