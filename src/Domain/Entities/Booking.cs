﻿
using Domain.Enums;

namespace Domain.Entities
{
    public class Booking : EntityKey
    {
        public Guid GuestId { get; set; }
        public User Guest { get; set; }
        public Guid HotelId { get; set; }
        public Hotel Hotel { get; set; }
        public ICollection<Room> Rooms { get; set; } = new List<Room>();
        public ICollection<InvoiceRecord> Invoice { get; set; } = new List<InvoiceRecord>();
        public decimal TotalPrice { get; set; }
        public DateOnly CheckIn { get; set; }
        public DateOnly CheckOut { get; set; }
        public DateOnly BookingDate { get; set; }
        public string? GuestRemarks { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }
}
