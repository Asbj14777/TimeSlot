using TimeSlot.Models;

namespace TimeSlot.Interfaces
{
    public interface IBookingService
    {
        Task Add(Booking booking);
        Task Delete(int id);
        List<Booking> GetAll();
        Booking? GetById(int id);
        Task Update(Booking booking);
    }
}
