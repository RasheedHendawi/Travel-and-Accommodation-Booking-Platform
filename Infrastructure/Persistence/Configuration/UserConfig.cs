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
            builder.HasData
                ([
                    new User
                    {
                        Id = new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a"),
                        FirstName = "Super",
                        LastName = "Admin",
                        Email = "Admin@hotelManager.com",
                        Password= "AQAAAAIAAYagAAAAEMcAhymzoRbYY1s8WP2AWcrQV3CHk35ny+1XHcuYxyfVqKIy5IaRVHzHa4SqBJOzFQ=="
                    }
                ]);
            builder.HasMany(u => u.Roles)
              .WithMany(r => r.Users)
              .UsingEntity<Dictionary<string, object>>(
                "UserRole",
                j => j.HasOne<Role>().WithMany()
                  .HasForeignKey("RoleId").OnDelete(DeleteBehavior.Cascade),
                j => j.HasOne<User>().WithMany()
                  .HasForeignKey("UserId").OnDelete(DeleteBehavior.Cascade))
                  .HasData([new Dictionary<string, object>{
                        ["UserId"] = new Guid("f403d2ed-7f0d-499c-a6e9-7e3d751f842a"),
                        ["RoleId"] = new Guid("c23401af-cbb8-4b73-8d6f-ecbb5e31d3b7")
                    }]);


            builder.HasIndex(u => u.Email).IsUnique();
        }
    }
}
