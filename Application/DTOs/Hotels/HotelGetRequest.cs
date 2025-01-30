using Domain.Enums;

namespace Application.DTOs.Hotels
{
    public class HotelGetRequest
    {
        public string? SearchTerm { get; set; }
        public string? SortColumn { get; set; }
        public SortMethod? SortOrder { get; set; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }
}
