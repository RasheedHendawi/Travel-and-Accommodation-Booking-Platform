using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.Persistence.Configuration
{
    public class RoleConfig : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);
            builder.HasMany(r => r.Users)
                .WithMany(u => u.Roles);

            builder.HasData
                ([
                    new Role {Id = new Guid("bd0cb72e-df0a-4514-99d8-52bb81b41340"), Name = "Guest" },
                    new Role {Id = new Guid("c23401af-cbb8-4b73-8d6f-ecbb5e31d3b7"), Name = "Admin" }
                ]);

        }
    }
}
