

namespace Application.DTOs.Hotels
{
    public class HotelCreationRequest
    {
        public Guid CityId { get; init; }
        public Guid OwnerId { get; init; }
        public string Name { get; init; }
        public int StartRating { get; init; }
        public double Longitude { get; init; }
        public double Latitude { get; init; }
        public string? BriefDescription { get; init; }
        public string? Description { get; init; }
        public string PhoneNubmer { get; init; }
    }
}
