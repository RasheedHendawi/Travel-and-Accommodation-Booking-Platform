

namespace Application.DTOs.RoomClass
{
    public class RoomClassUpdateRequest
    {
        public string Name { get; init; }
        public string? Description { get; init; }
        public int AdultCapacity { get; init; }
        public int ChildCapacity { get; init; }
        public decimal Price { get; init; }
    }
}
