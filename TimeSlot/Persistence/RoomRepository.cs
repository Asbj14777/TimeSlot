using Microsoft.EntityFrameworkCore;
using TimeSlot.Data;
using TimeSlot.Interfaces;
using TimeSlot.Models;

namespace TimeSlot.Persistence
{
    public class RoomRepository : IRoomRepository
    {
        private readonly TimeSlotContext _context;

        public RoomRepository(TimeSlotContext context)
        {
            _context = context;
        }

        public async void Add(Room room)
        {
            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();
        }

        public async void Delete(int id)
        {
            var room = _context.Rooms.FirstOrDefault(x => x.RoomId == id);

            if (room != null)
            {
                _context.Rooms.Remove(room);
                _context.SaveChanges();
            }
        }

        public List<Room> GetAll()
        {
            return _context.Rooms.ToList();
        }

        public Room? GetById(int id)
        {
            return _context.Rooms
                .Include(r => r.Bookings)
                .FirstOrDefault(r => r.RoomId == id);
        }

        public async void Update(Room room)
        {
            _context.Rooms.Update(room);
            await _context.SaveChangesAsync();
        }
    }
}