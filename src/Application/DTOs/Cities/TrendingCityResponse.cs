namespace Application.DTOs.Cities
{
    public class TrendingCityResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public string Country { get; init; }
        public string? ThumbnailUrl { get; init; }
    }
}
