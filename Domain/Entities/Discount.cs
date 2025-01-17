

namespace Domain.Entities
{
    public class Discount : EntityKey
    {
        public Guid RoomClassId { get; set; }
        public RoomClass RoomClass { get; set; }
        public decimal Percentage { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
