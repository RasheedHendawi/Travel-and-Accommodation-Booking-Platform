using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(u => u.Id);
            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users);

            builder.HasMany(u => u.Roles)
                .WithMany(r => r.Users).UsingEntity<Dictionary<string, object>>(
                "UserRole",
                a => a.HasOne<Role>().WithMany().HasForeignKey("RoleId")
                .OnDelete(DeleteBehavior.Cascade),
                a => a.HasOne<User>().WithMany().HasForeignKey("UserId")
                .OnDelete(DeleteBehavior.Cascade));




            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
