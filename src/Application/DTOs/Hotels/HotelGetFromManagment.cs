using Application.DTOs.Shared;

namespace Application.DTOs.Hotels
{
    public class HotelGetFromManagment
    {
        public Guid Id { get; init; }
        public string Name { get; init; }
        public int StarRating { get; init; }
        public OwnersResponse Owner { get; init; }
        public int NumberOfRooms { get; init; }
        public DateTime CreatedAt { get; init; }
        public DateTime? UpdatedAt { get; init; }
    }
}
