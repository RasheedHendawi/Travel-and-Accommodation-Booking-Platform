

namespace Application.DTOs.Discounts
{ 
    public class DiscountResponse
    {
        public Guid Id { get; init; }
        public Guid RoomClassId { get; init; }
        public float Percentage { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
        public DateTime CreatedAt { get; init; }
    }
}
