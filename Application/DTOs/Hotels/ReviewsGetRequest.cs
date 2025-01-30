using Domain.Enums;

namespace Application.DTOs.Hotels
{
    public class ReviewsGetRequest
    {
        public string? SortColumn { get; init; }
        public SortMethod? SortOrder { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
