using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TimeSlot.Data;
using TimeSlot.Interfaces;
using TimeSlot.Models;
using TimeSlot.ViewModels;

namespace TimeSlot.Controllers
{
    [Authorize]
    public class BookingsController : Controller
    {
        private readonly IRoomRepository roomRepository;
        private readonly IBookingService bookingService;
        private readonly UserManager<ApplicationUser> _userManager;

        public BookingsController(
            IRoomRepository roomRepository,
            IBookingService bookingService,
            UserManager<ApplicationUser> userManager)
        {
            this.roomRepository = roomRepository;
            this.bookingService = bookingService;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            var bookings = bookingService.GetAll()
                .Where(b => b.StartTime > DateTime.Now);

            if (!User.IsInRole("Admin"))
            {
                var userId = _userManager.GetUserId(User);
                bookings = bookings.Where(b => b.UserId == userId);
            }

            return View(bookings.OrderBy(b => b.StartTime).ToList());
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
                date.Year, date.Month, date.Day, date.Hour, date.Minute, 0);
            bookingVM.Booking.EndTime = bookingVM.Booking.StartTime.AddHours(1);

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
                bookingVM.Booking.UserId = _userManager.GetUserId(User);
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

            if (!CanManageBooking(booking))
            {
                return Forbid();
            }

            var bookingVM = new BookingViewModel
            {
                Booking = booking,
                Rooms = roomRepository.GetAll()
            };

            ViewBag.Action = "edit";
            return View(bookingVM);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(BookingViewModel bookingVM)
        {
            ViewBag.Action = "edit";

            var existingBooking = bookingService.GetById(bookingVM.Booking.BookingId);
            if (existingBooking == null)
            {
                return NotFound();
            }

            if (!CanManageBooking(existingBooking))
            {
                return Forbid();
            }

            // The owner is taken from the database, never from posted form data.
            bookingVM.Booking.UserId = existingBooking.UserId;

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
            catch (DbUpdateConcurrencyException)
            {
                ModelState.AddModelError(
                    "",
                    "This booking was changed by another user. Please review the booking and try again.");

                bookingVM.Rooms = roomRepository.GetAll();
                return View(bookingVM);
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
            var booking = bookingService.GetById(id);
            if (booking == null)
            {
                return NotFound();
            }

            if (!CanManageBooking(booking))
            {
                return Forbid();
            }

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

        private bool CanManageBooking(Booking booking)
        {
            return User.IsInRole("Admin") ||
                   booking.UserId == _userManager.GetUserId(User);
        }
    }
}
