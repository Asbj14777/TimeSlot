  using Microsoft.EntityFrameworkCore;
using TimeSlot.Models;
using Microsoft.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
namespace TimeSlot.Data
{
    public class TimeSlotContext : IdentityDbContext<ApplicationUser>
    {

       public DbSet<Room> Rooms { get; set; }
       public DbSet<Booking> Bookings { get; set; }

       public TimeSlotContext(DbContextOptions<TimeSlotContext> options) : base(options) {
            
       }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Room)
                .WithMany(r => r.Bookings)
                .HasForeignKey(b => b.RoomId);
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(r => r.Bookings);
                

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

            modelBuilder.Entity<Booking>().HasData(
                new Booking
                {
                    BookingId = 1,
                    Title = "Projektmøde",
                    StartTime = new DateTime(2026, 9, 8, 10, 0, 0),
                    EndTime = new DateTime(2026, 9, 8, 11, 0, 0),
                    RoomId = 1
                },
                new Booking
                {
                    BookingId = 2,
                    Title = "Gruppemøde",
                    StartTime = new DateTime(2026, 9, 8, 12, 0, 0),
                    EndTime = new DateTime(2026, 9, 8, 13, 0, 0),
                    RoomId = 2
                }
            );
            base.OnModelCreating(modelBuilder);
        }
    }
}
