using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TimeSlot.Models;

namespace TimeSlot.Data
{
    public class TimeSlotContext : IdentityDbContext<ApplicationUser>
    {
        public DbSet<Room> Rooms { get; set; }
        public DbSet<Booking> Bookings { get; set; }

        public TimeSlotContext(DbContextOptions<TimeSlotContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.User)
                .WithMany(u => u.Bookings)
                .HasForeignKey(b => b.UserId);

            modelBuilder.Entity<Room>().HasData(
                new Room
                {
                    RoomId = 1,
                    Name = "Mødelokale 1",
                    Capacity = 10
                },
                new Room
                {
                    RoomId = 2,
                    Name = "Mødelokale 2",
                    Capacity = 20
                }
            );
        }
    }
}
