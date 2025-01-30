

namespace Application.DTOs.Hotels
{
    public class HotelGetResponse
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public string CityName { get; init; }
        public string CountryName { get; init; }
        public int StartRating { get; init; }
        public float ReviewsRating { get; init; }
        public string? BriefDescription { get; init; }
        public string? Description { get; init; }
        public string? ThumbnailUrl { get; init; }
        public IEnumerable<string> GalleryUrls { get; init; }
    }
}
