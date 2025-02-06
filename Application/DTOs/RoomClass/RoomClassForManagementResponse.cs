

using Application.DTOs.Amenities;
using Application.DTOs.Discounts;

namespace Application.DTOs.RoomClass
{
    public class RoomClassForManagementResponse
    {
        public Guid RoomClassId { get; init; }
        public string Name { get; init; }
        public string? Description { get; init; }
        public int AdultCapacity { get; init; }
        public int ChildCapacity { get; init; }
        public decimal Price { get; init; }
        public IEnumerable<AmenityResponse> Amenities { get; set; }
        public DiscountResponse? ActiveDiscount { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
