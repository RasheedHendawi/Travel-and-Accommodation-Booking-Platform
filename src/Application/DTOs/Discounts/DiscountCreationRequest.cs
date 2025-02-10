

namespace Application.DTOs.Discounts
{
    public class DiscountCreationRequest
    {
        public decimal Percentage { get; init; }
        public DateTime StartDate { get; init; }
        public DateTime EndDate { get; init; }
    }
}
