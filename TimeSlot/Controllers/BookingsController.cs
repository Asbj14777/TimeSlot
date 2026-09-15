using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using TimeSlot.Data;
using TimeSlot.Interfaces;

using TimeSlot.Models;
using TimeSlot.ViewModels;

namespace TimeSlot.Controllers
{

    public class BookingsController : Controller
    {
        private readonly IRoomRepository roomRepository;
        private readonly IBookingService bookingService;
        private readonly UserManager<ApplicationUser> _userManager; 
        public BookingsController(
            IRoomRepository roomRepository,
            IBookingService bookingService, UserManager<ApplicationUser> userManager)
        {
            this.roomRepository = roomRepository;
            this.bookingService = bookingService;
            _userManager = userManager; 
        }

       
        public IActionResult Index()
        {
            var bookings = bookingService.GetAll();

            return View(bookings);
        }

        public IActionResult Add(int? id)
        {
            ViewBag.Action = "add";

            var bookingVM = new BookingViewModel
            {
                Rooms = roomRepository.GetAll()
            };

            var date = DateTime.Now;

            bookingVM.Booking.StartTime = new DateTime(
                date.Year,
                date.Month,
                date.Day,
                date.Hour,
                date.Minute,
                0);

            bookingVM.Booking.EndTime =
                bookingVM.Booking.StartTime.AddHours(1);

            // If a room was selected beforehand
            if (id != null)
            {
                bookingVM.Booking.RoomId = id.Value;
            }

            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Add(BookingViewModel bookingVM)
        {
            ViewBag.Action = "add";

            if (!ModelState.IsValid)
            {
                bookingVM.Rooms = roomRepository.GetAll();

                return View(bookingVM);
            }

            try
            {
                bookingService.Add(bookingVM.Booking);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                bookingVM.Rooms = roomRepository.GetAll();

                return View(bookingVM);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var booking = bookingService.GetById(id.Value);

            if (booking == null)
            {
                return NotFound();
            }

            var bookingVM = new BookingViewModel
            {
                Booking = booking,
                Rooms = roomRepository.GetAll()
            };

            ViewBag.Action = "edit";

            return View(bookingVM);
        }

        // POST: Bookings/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookingViewModel bookingVM)
        {
            ViewBag.Action = "edit";

            if (!ModelState.IsValid)
            {
                bookingVM.Rooms = roomRepository.GetAll();

                return View(bookingVM);
            }

            try
            {
                bookingService.Update(bookingVM.Booking);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                bookingVM.Rooms = roomRepository.GetAll();

                return View(bookingVM);
            }
        }

        public IActionResult Delete(int id)
        {
            try
            {
                bookingService.Delete(id);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", ex.Message);

                return RedirectToAction(nameof(Index));
            }
        }
    }
}