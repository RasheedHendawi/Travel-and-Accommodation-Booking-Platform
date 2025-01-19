using Domain.Entities;
using Infrastructure.Persistence.Configuration;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Persistence.ContextDb
{
    public class HotelBookingPlatformDbContext : DbContext
    {
        public HotelBookingPlatformDbContext(DbContextOptions<HotelBookingPlatformDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<Discount> Discounts { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<Room> Rooms { get; set; }
        public DbSet<RoomClass> RoomClasses { get; set; }
        public DbSet<Hotel> Hotels { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<Amenity> Amenities { get; set; }
        public DbSet<City> Cities { get; set; }
        public DbSet<Image> Images { get; set; }
        public DbSet<Owner> Owners { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.ApplyConfigurationsFromAssembly(typeof(HotelBookingPlatformDbContext).Assembly);
            modelBuilder.ApplyConfiguration(new ReviewConfig());
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new RoleConfig());
            modelBuilder.ApplyConfiguration(new DiscountConfig());
            modelBuilder.ApplyConfiguration(new RoomConfig());
            modelBuilder.ApplyConfiguration(new RoomClassConfig());
            modelBuilder.ApplyConfiguration(new HotelConfig());
            modelBuilder.ApplyConfiguration(new BookingConfig());
            modelBuilder.ApplyConfiguration(new AmenityConfig());
            modelBuilder.ApplyConfiguration(new CityConfig());
            modelBuilder.ApplyConfiguration(new OwnerConfig());
            modelBuilder.ApplyConfiguration(new ImageConfig());
            modelBuilder.ApplyConfiguration(new InvoiceConfig());
        }
    }
}
















//modelBuilder.ApplyConfiguration(new UserConfig());
//modelBuilder.ApplyConfiguration(new RoleConfig());
//modelBuilder.ApplyConfiguration(new DiscountConfig());
//modelBuilder.ApplyConfiguration(new ReviewConfig());
//modelBuilder.ApplyConfiguration(new RoomConfig());
//modelBuilder.ApplyConfiguration(new RoomClassConfig());
//modelBuilder.ApplyConfiguration(new HotelConfig());
//modelBuilder.ApplyConfiguration(new BookingConfig());
//modelBuilder.ApplyConfiguration(new AmenityConfig());
//modelBuilder.ApplyConfiguration(new CityConfig());
//modelBuilder.ApplyConfiguration(new OwnerConfig());
//modelBuilder.ApplyConfiguration(new ImageConfig());
//modelBuilder.ApplyConfiguration(new InvoiceConfig());