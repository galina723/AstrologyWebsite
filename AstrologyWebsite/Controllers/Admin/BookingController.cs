using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Booking")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly AstrologyDatabaseContext _context;
        private readonly UserManager<AstroUser> _userManager;

        public BookingController(AstrologyDatabaseContext context, UserManager<AstroUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [HttpGet]
        public async Task<IActionResult> BookingManagement()
        {
            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .Include(b => b.Tarot)
                .ToListAsync();
            return View(bookings);
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> EditBooking(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .Include(b => b.Tarot)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            ViewBag.TarotReaders = await _userManager.GetUsersInRoleAsync("TarotReader");
            return View(booking);
        }

        [HttpPost("{id}")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBooking(int id, Booking updated)
        {
            var booking = await _context.Bookings
                .Include(b => b.Service)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            var tarotId = updated.TarotId;
            var scheduleAt = booking.ScheduleAt;
            var duration = booking.Service?.Duration ?? 0;

            var endTime = scheduleAt?.AddMinutes(duration);
            var conflict = await _context.Bookings
                .Where(b => b.TarotId == tarotId && b.Id != id && b.ScheduleAt.HasValue && b.Service != null)
                .AnyAsync(b =>
                    b.ScheduleAt < endTime &&
                    b.ScheduleAt.Value.AddMinutes(b.Service.Duration ?? 0) > scheduleAt);

            if (conflict)
            {
                ModelState.AddModelError("TarotId", "This tarot reader is not available at the selected time.");
                ViewBag.TarotReaders = await _userManager.GetUsersInRoleAsync("TarotReader");
                return View(booking);
            }

            booking.TarotId = tarotId;
            booking.Status = updated.Status;
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Booking updated successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(BookingManagement));
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var booking = await _context.Bookings.FindAsync(id);
            if (booking == null)
                return NotFound();

            if (booking.Status != BookingStatus.In_Progress || !string.IsNullOrEmpty(booking.TarotId))
                return Forbid();

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();
            TempData["ToastMessage"] = "Booking deleted successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction(nameof(BookingManagement));
        }
    }
}
