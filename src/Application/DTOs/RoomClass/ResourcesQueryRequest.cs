

using Domain.Enums;

namespace Application.DTOs.RoomClass
{
    public class ResourcesQueryRequest
    {
        public string? SearchTerm { get; init; }
        public SortMethod? SortOrder { get; init; }
        public string? SortColumn { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
