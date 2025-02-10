
namespace Domain.Entities
{
    public class City : EntityKey, IAuditableEntity
    {
        public string Name { get; set; }
        public string Country { get; set; }
        public string PostOffice { get; set; }
        public ICollection<Hotel> Hotels { get; set; } = new List<Hotel>();
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public Image? Thumbnail { get; set; }
    }
}
