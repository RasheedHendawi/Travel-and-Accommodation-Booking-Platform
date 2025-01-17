using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class OwnerConfig : IEntityTypeConfiguration<Owner>
    {
        public void Configure(EntityTypeBuilder<Owner> builder)
        {
            builder.HasKey(o => o.Id);
            builder.HasMany(o => o.Hotels).WithOne(p => p.Owner)
                .HasForeignKey(p => p.OwnerId)
                .OnDelete(DeleteBehavior.Cascade)
                .IsRequired();
        }
    }
}
