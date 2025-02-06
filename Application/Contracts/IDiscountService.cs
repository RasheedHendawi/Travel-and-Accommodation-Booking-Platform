

using Application.DTOs.Discounts;
using Domain.Models;

namespace Application.Contracts
{
    public interface IDiscountService
    {
        Task<PaginatedList<DiscountResponse>> GetAmenitiesAsync(Guid roomClassId, DiscountsGetRequest request);
        Task<DiscountResponse> GetDiscountByIdAsync(Guid roomClassId, Guid discountId);
        Task<DiscountResponse> CreateDiscountAsync(Guid roomClassId, DiscountCreationRequest request);
        Task DeleteDiscountAsync(Guid roomClassId, Guid discountId);
    }
}
