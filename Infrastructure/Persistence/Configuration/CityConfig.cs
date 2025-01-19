using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class CityConfig : IEntityTypeConfiguration<City>
    {
        public void Configure(EntityTypeBuilder<City> builder)
        {
            builder.HasKey(c => c.Id);
            builder.HasMany(c => c.Hotels)
                .WithOne(r => r.City)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict);
            builder.Ignore(a => a.Thumbnail);
        }
    }
}
