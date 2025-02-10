using Domain.Enums;

namespace Application.DTOs.Hotels
{
    public class GetReviewsByIdResponse
    {
        public Guid HotelId { get; init; }
        public SortMethod? SortOrder { get; init; }
        public string? SortColumn { get; init; }
        public int PageNumber { get; init; }
        public int PageSize { get; init; }
    }
}
