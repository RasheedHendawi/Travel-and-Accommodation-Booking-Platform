using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class HotelConfig : IEntityTypeConfiguration<Hotel>
    {
        public void Configure(EntityTypeBuilder<Hotel> builder)
        {
            builder.HasKey(h => h.Id);

            builder.HasMany(s => s.RoomClasses)
                .WithOne(r => r.Hotel)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(h => h.Bookgins)
                .WithOne(b => b.Hotel)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.Ignore(s => s.Gallery);
            builder.Ignore(s => s.Thumbnail);

            builder.Property(s => s.Latitude)
                .HasPrecision(8, 6);
            builder.Property(s => s.Longitude).HasPrecision(8, 6);
            builder.Property(s => s.ReviewsRating)
                .HasPrecision(8, 6);
            builder.HasIndex(s => s.StartRating);
        }
    }
}
