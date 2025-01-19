using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class InvoiceConfig : IEntityTypeConfiguration<InvoiceRecord>  
    {
        public void Configure(EntityTypeBuilder<InvoiceRecord> builder)
        {
            builder.HasKey(i => i.Id);
            builder.Property(i => i.Price)
                .HasPrecision(18, 2);
            builder.Property(i => i.DiscountPercentage)
                .HasPrecision(18, 2);
        }
    }
}
