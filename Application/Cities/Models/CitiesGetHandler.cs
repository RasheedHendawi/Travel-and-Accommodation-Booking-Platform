using Domain.Enums;


namespace Application.Cities.Models
{
    public class CitiesGetHandler
    {
        public string? SearchTerm { get; init; }
        public SortMethod? SortOrder { get; init; }
        public string? SortColumn { get; init; }
        public int PageNumber { get; init; } = 1;
        public int PageSize { get; init; } = 10;
    }
}
