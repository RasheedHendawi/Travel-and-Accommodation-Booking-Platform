
namespace Domain.Entities
{
    public class Amenity : EntityKey
    {
        public string Name { get; set; }
        public string? Description { get; set; }
        public ICollection<RoomClass> RoomClasses { get; set; } = new List<RoomClass>();
    }
}
