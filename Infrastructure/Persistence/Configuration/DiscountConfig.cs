using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;


namespace Infrastructure.Persistence.Configuration
{
    public class DiscountConfig : IEntityTypeConfiguration<Discount>
    {
        public void Configure(EntityTypeBuilder<Discount> builder)
        {
            builder.HasKey(d => d.Id);
            builder.Property(d => d.Percentage)
                .HasColumnType("decimal(18,2)");
            //builder.HasIndex(d => new { d.StartDate, d.EndDate });
        }
    }
}
