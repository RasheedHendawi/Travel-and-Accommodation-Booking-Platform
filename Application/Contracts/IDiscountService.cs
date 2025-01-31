

using Application.DTOs.Discounts;

namespace Application.Contracts
{
    public interface IDiscountService
    {
        Task<IEnumerable<DiscountResponse>> GetAmenitiesAsync(Guid roomClassId, DiscountsGetRequest request);
        Task<DiscountResponse> GetDiscountByIdAsync(Guid roomClassId, Guid discountId);
        Task<DiscountResponse> CreateDiscountAsync(Guid roomClassId, DiscountCreationRequest request);
        Task DeleteDiscountAsync(Guid roomClassId, Guid discountId);
    }
}
