using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Entities
{
    public class InvoiceRecord : EntityKey
    {
        public Guid BookingId { get; set; }
        public Booking Booking { get; set; }
        public Guid RoomId { get; set; }
        public string RoomClassName { get; set; }
        public string RoomName { get; set;
        public decimal Price { get; set; }
        public decimal? DiscountPercentage { get; set; }
    }
}
