
using TimeSlot.Interfaces;
using TimeSlot.Models;

namespace TimeSlot.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository bookingRepository;
        private readonly IRoomRepository roomRepository;

        public BookingService(IBookingRepository bookingRepository, IRoomRepository roomRepository)
        {
            this.bookingRepository = bookingRepository;
            this.roomRepository = roomRepository;
        }

        public Booking? GetById(int id)
        {
            return bookingRepository.GetById(id);
        }

        public List<Booking> GetAll()
        {
            return bookingRepository.GetAll();
        }

        public void Add(Booking booking)
        {
            if (booking.EndTime <= booking.StartTime)
            {
                throw new Exception("End time must be after start time.");
            }

            if (booking.StartTime <= DateTime.Now)
            {
                throw new Exception("Booking must start in the future.");
            }

            var room = roomRepository.GetById(booking.RoomId);

            if (room == null)
            {
                throw new Exception("Room not found.");
            }

            foreach (var existingBooking in room.Bookings)
            {
                bool overlap = booking.StartTime < existingBooking.EndTime &&
                               booking.EndTime > existingBooking.StartTime;

                if (overlap)
                {
                    throw new Exception("The room is already booked during this time.");
                }
            }

            bookingRepository.Add(booking);
        }

        public void Update(Booking booking)
        {
            if (booking.EndTime <= booking.StartTime)
            {
                throw new Exception("End time must be after start time.");
            }

            if (booking.StartTime <= DateTime.Now)
            {
                throw new Exception("Booking must start in the future.");
            }

            var room = roomRepository.GetById(booking.RoomId);

            if (room == null)
            {
                throw new Exception("Room not found.");
            }

            foreach (var existingBooking in room.Bookings)
            {
                if (existingBooking.BookingId == booking.BookingId)
                {
                    continue;
                }

                bool overlap = booking.StartTime < existingBooking.EndTime &&
                               booking.EndTime > existingBooking.StartTime;

                if (overlap)
                {
                    throw new Exception("The room is already booked during this time.");
                }
            }

            bookingRepository.Update(booking);
        }

        public void Delete(int id)
        {
            var booking = bookingRepository.GetById(id);

            if (booking == null)
            {
                throw new Exception("Booking not found.");
            }

            bookingRepository.Delete(id);
        }
    }
}
