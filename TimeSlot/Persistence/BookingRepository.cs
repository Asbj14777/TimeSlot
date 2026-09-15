using TimeSlot.Data;
using TimeSlot.Interfaces;
using TimeSlot.Models;
using Microsoft.EntityFrameworkCore;
namespace TimeSlot.Persistence
{
    public class BookingRepository : IBookingRepository
    {


        private readonly TimeSlotContext _context;

        public BookingRepository(TimeSlotContext context)
        {
            _context = context;
        }

        public void Add(Booking booking)
        {
          
            
                _context.Bookings.Add(booking);
                _context.SaveChanges();
            
        }
        public void Delete(int id)
        {
         
                var booking = _context.Bookings.FirstOrDefault(x => x.BookingId == id);
                if (booking != null)
                {
                    _context.Bookings.Remove(booking);
                    _context.SaveChanges();
                }
            
        }
        public List<Booking> GetAll()
        {
            return _context.Bookings
                .Include(b => b.Room)
                .ToList();
        }
        public Booking? GetById(int id)
        {


            return _context.Bookings
          .Include(b => b.Room)
          .ThenInclude(r => r.Bookings)
          .FirstOrDefault(b => b.BookingId == id);
        }
        public void Update(Booking booking)
        {

                _context.Bookings.Update(booking);
                _context.SaveChanges();
           
        }
    }
}
