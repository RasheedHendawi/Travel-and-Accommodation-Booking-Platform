using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
namespace Infrastructure.Persistence.Configuration
{
    public class BookingConfig : IEntityTypeConfiguration<Booking>
    {
        public void Configure(EntityTypeBuilder<Booking> builder)
        {
            builder.HasKey(b => b.Id);

            builder.HasMany(b => b.Rooms)
                .WithMany(r => r.Bookings);

            builder.HasMany(b => b.Invoice)
                .WithOne(i => i.Booking)
                .IsRequired()
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(b => b.TotalPrice)
                .HasPrecision(18, 2);

            builder.Property(a => a.PaymentMethod)
                .HasConversion(
                new EnumToStringConverter<PaymentMethod>());
            builder.HasIndex(a => new { a.CheckIn, a.CheckOut });
        }
    }
}
