using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Controllers.Admin
{
    [Authorize(Roles = "TarotReader")]
    public class ScheduleController : Controller
    {
        private readonly AstrologyDatabaseContext _context;
        private readonly UserManager<AstroUser> _userManager;

        public ScheduleController(AstrologyDatabaseContext context, UserManager<AstroUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IActionResult> MySchedule()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
                return Unauthorized();

            var bookings = await _context.Bookings
                .Include(b => b.Customer)
                .Include(b => b.Service)
                .Where(b => b.TarotId == user.Id)
                .OrderByDescending(b => b.ScheduleAt)
                .ToListAsync();

            return View(bookings);
        }
    }

}
