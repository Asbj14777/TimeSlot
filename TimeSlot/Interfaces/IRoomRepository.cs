using TimeSlot.Models;

namespace TimeSlot.Interfaces
{
    public interface IRoomRepository
    {
        Task Add(Room room);
        Task Delete(int id);
        List<Room> GetAll();
        Room? GetById(int id);
        Task Update(Room room);
    }

}
