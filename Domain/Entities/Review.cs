using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class Review : EntityKey , IAuditableEntity
    {
        public Guid GuestId { get; set; }
        public User Guest { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public string Content { get; set; }
        public int Rating { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
    }
}
