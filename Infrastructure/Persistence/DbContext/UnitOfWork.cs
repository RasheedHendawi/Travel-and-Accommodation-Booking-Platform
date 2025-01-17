using Domain.Entities;
using Domain.Interfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace Infrastructure.Persistence.DbContext
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly HotelBookingPlatformDbContext _context;
        public UnitOfWork(HotelBookingPlatformDbContext context)
        {
            _context = context;
        }
        public async Task BeginTransactionAsync()
        {
            await _context.Database.BeginTransactionAsync(IsolationLevel.Serializable);
        }


        public async Task CommitTransactionAsync()
        {
            if (_context.Database.CurrentTransaction is null) return;

            await _context.Database.CommitTransactionAsync();
        }
        public async Task RollbackTransactionAsync()
        {
            if (_context.Database.CurrentTransaction is null) return;

            await _context.Database.RollbackTransactionAsync();
        }
        public async Task<int> SaveChangesAsync()
        {
            _context.ChangeTracker.DetectChanges();

            foreach (var entry in _context.ChangeTracker.Entries<IAuditableEntity>())
                switch (entry.State)
                {
                    case EntityState.Added:
                        entry.Entity.CreatedAt = DateTime.UtcNow;
                        break;
                    case EntityState.Modified:
                        entry.Entity.UpdatedAt = DateTime.UtcNow;
                        break;
                }

            return await _context.SaveChangesAsync();
        }

    }
}
