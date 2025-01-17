using Domain.Entities;
using Domain.Enums;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Infrastructure.Persistence.Configuration
{
    public class RoomClassConfig : IEntityTypeConfiguration<RoomClass>
    {
        public void Configure(EntityTypeBuilder<RoomClass> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.RoomType)
              .HasConversion(new EnumToStringConverter<RoomType>());

            builder.Ignore(h => h.Gallary);

            builder.HasMany(rc => rc.Rooms).WithOne(r => r.RoomClass)
              .IsRequired()
              .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(rc => rc.Amenities)
              .WithMany(a => a.RoomClasses);

            builder.Property(rc => rc.Price)
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(rc => rc.RoomType);

            builder.HasIndex(rc => new { rc.AdultCapacity, rc.ChildCapacity });

            builder.HasIndex(r => r.Price);
        }
    }
}
