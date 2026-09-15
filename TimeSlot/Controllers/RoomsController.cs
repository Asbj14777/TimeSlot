using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TimeSlot.Interfaces; 
using TimeSlot.Persistence;

namespace TimeSlot.Controllers
{
 
    public class RoomsController : Controller
    {
        private readonly IRoomRepository roomRepository; 
        public RoomsController(IRoomRepository roomRepository)
        {
            this.roomRepository = roomRepository;
        }
        [Authorize]
        public IActionResult Index()
        {
            var rooms = roomRepository.GetAll();
            return View(rooms);
        }
    }
}
