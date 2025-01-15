using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Room : EntityKey, IAuditableEntity
    {
        public Guid RoomClassId { get; set; }
        public RoomClass RoomClass { get; set; }
        public string Number { get; set; }
        public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
        public ICollection<InvoiceRecord> InvoiceRecords { get; set; } = new List<InvoiceRecord>();
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

    }
}
