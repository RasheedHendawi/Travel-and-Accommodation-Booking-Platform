using Domain.Enums;

namespace Domain.Entities
{
    public class RoomClass : EntityKey, IAuditableEntity
    {
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public int AdultCapacity { get; set; }
        public int ChildCapacity { get; set; }
        public decimal Price { get; set; }
        public RoomType RoomType { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<Amenity> Amenities { get; set; } = new List<Amenity>();
        public ICollection<Image> Gallary { get; set; } = new List<Image>();
        public ICollection<Discount> Discounts { get; set; } = new List<Discount>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
