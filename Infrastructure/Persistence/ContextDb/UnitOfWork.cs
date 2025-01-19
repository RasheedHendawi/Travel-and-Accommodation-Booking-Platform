using Domain.Entities;
using Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.ContextDb
{
    public class UnitOfWork(HotelBookingPlatformDbContext context) : IUnitOfWork
    {

        public async Task BeginTransactionAsync()
        {
            await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }


        public async Task CommitTransactionAsync()
        {
            if (context.Database.CurrentTransaction is null) return;

            await context.Database.CommitTransactionAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            if (context.Database.CurrentTransaction is null) return;

            await context.Database.RollbackTransactionAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
                context.ChangeTracker.DetectChanges();

            foreach (var entry in context.ChangeTracker.Entries<IAuditableEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }

            return await context.SaveChangesAsync();
        }

    }
}
