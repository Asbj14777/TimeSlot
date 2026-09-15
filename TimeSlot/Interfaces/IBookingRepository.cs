using TimeSlot.Models;

namespace TimeSlot.Interfaces
{
    public interface IBookingRepository
    {
        void Add(Booking booking);
        void Delete(int id);
        List<Booking> GetAll();
        Booking? GetById(int id);
        void Update(Booking booking);
    }

}
