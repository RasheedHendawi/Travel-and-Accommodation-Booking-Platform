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
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
            builder.Property(s => s.Latitude)
                .HasColumnType("decimal(18,2)");
            builder.Property(s => s.Longitude)
                .HasColumnType("decimal(18,2)");
            builder.Property(s => s.ReviewsRating)
                .HasColumnType("decimal(18,2)");
            builder.Ignore(s => s.Gallery);
            builder.Ignore(s => s.Thumbnail);
            builder.HasIndex(s => s.StartRating);
        }
    }
}
