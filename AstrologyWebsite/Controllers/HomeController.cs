using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using AstrologyWebsite.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using AstrologyWebsite.ViewModels;

namespace AstrologyWebsite.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AstrologyDatabaseContext _context;
    private readonly UserManager<AstroUser> _userManager;
    public HomeController(ILogger<HomeController> logger, AstrologyDatabaseContext context, UserManager<AstroUser> userManager)
    {
        _logger = logger;
        _context = context;
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var blogs = _context.Blogs
         .OrderByDescending(b => b.CreatedDate)
         .Take(3)
         .ToList();

        ViewBag.Blogs = blogs;
        var banners = await _context.Banners.ToListAsync();
        ViewBag.Banners = banners;
        return View();
    }


    //Blog
    public async Task<IActionResult> Blog()
    {
        var blogs = await _context.Blogs
            .Include(b => b.Author)
            .OrderByDescending(b => b.CreatedDate)
            .ToListAsync();
        return View(blogs);
    }
    public async Task<IActionResult> BlogDetail(int id)
    {
        var blog = await _context.Blogs
            .Include(b => b.Author)
            .FirstOrDefaultAsync(b => b.Id == id);

        if (blog == null)
            return NotFound();

        return View(blog);
    }

    //Blog end

    //Service
    public async Task<IActionResult> Service()
    {
        var services = await _context.Services.ToListAsync();
        return View(services);
    }
    public async Task<IActionResult> ServiceDetails(int id)
    {
        var service = await _context.Services.FirstOrDefaultAsync(s => s.Id == id);
        if (service == null)
        {
            return NotFound();
        }
        return View(service);
    }
    //Service End
    public IActionResult About()
    {
        return View();
    }

    //Booking
    [Authorize(Roles = "User")]
    public async Task<IActionResult> Booking(int? serviceId)
    {
        if (!User.Identity.IsAuthenticated)
        {
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Booking", "Home", new { serviceId }) });
        }

        var user = await _userManager.GetUserAsync(User);
        Booking booking = new Booking();

        if (serviceId.HasValue)
        {
            var service = await _context.Services.FindAsync(serviceId.Value);
            if (service != null)
            {
                booking.ServiceId = service.Id;
                booking.Price = service.Price;
                booking.Service = service;
            }
        }

        if (user != null)
        {
            booking.CustomerId = user.Id;
            booking.Customer = user;
        }

        return View(booking);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> AddBooking(Booking booking)
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null)
            return RedirectToAction("Login", "Account", new { returnUrl = Url.Action("Booking", "Home", new { serviceId = booking.ServiceId }) });

        booking.CustomerId = user.Id;
        booking.Status = BookingStatus.In_Progress;

        if (booking.ServiceId.HasValue)
            booking.Service = await _context.Services.FindAsync(booking.ServiceId.Value);

        if (!ModelState.IsValid)
        {
            booking.Customer = user;
            return View(booking);
        }

        _context.Bookings.Add(booking);
        await _context.SaveChangesAsync();
        TempData["BookingSuccess"] = "true";
        return RedirectToAction("Service", new { id = booking.Id });
    }

    //Booking End

    //MyAccount
    [HttpGet]
    public async Task<IActionResult> MyAccount()
    {
        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");
        var bookings = await _context.Bookings
        .Include(b => b.Service)
        .Include(b => b.Tarot)
        .Where(b => b.CustomerId == user.Id)
        .OrderByDescending(b => b.ScheduleAt)
        .ToListAsync();

        var model = new MyAccountViewModel
        {
            FullName = user.FullName,
            Gender = user.Gender,
            Dob = user.Dob,
            PhoneNumber = user.PhoneNumber,
            Email = user.Email,
            Image = user.Image,
            Bookings = bookings
        };
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> MyAccount(MyAccountViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = await _userManager.GetUserAsync(User);
        if (user == null) return RedirectToAction("Login", "Account");

        user.FullName = model.FullName;
        user.Gender = model.Gender;
        user.Dob = model.Dob;
        user.PhoneNumber = model.PhoneNumber;
        user.Email = model.Email;
        user.Image = model.Image;

        var updateResult = await _userManager.UpdateAsync(user);

        if (!string.IsNullOrWhiteSpace(model.NewPassword))
        {
            if (string.IsNullOrWhiteSpace(model.CurrentPassword))
            {
                ModelState.AddModelError("CurrentPassword", "Current password is required to change your password.");
                return View(model);
            }

            var passwordResult = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (!passwordResult.Succeeded)
            {
                foreach (var error in passwordResult.Errors)
                    ModelState.AddModelError("", error.Description);
                return View(model);
            }
        }

        if (updateResult.Succeeded)
        {
            ViewBag.Message = "Account updated successfully!";
        }
        else
        {
            foreach (var error in updateResult.Errors)
                ModelState.AddModelError("", error.Description);
        }

        return View(model);
    }
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteBooking(int id)
    {
        var user = await _userManager.GetUserAsync(User);
        var booking = await _context.Bookings.FirstOrDefaultAsync(b => b.Id == id && b.CustomerId == user.Id);

        if (booking == null || booking.Status != BookingStatus.In_Progress)
        {
            TempData["ErrorMessage"] = "Cannot delete this appointment.";
            return Redirect(Url.Action("MyAccount", "Home") + "#appointments");
        }

        _context.Bookings.Remove(booking);
        await _context.SaveChangesAsync();
        TempData["SuccessMessage"] = "Appointment deleted.";
        return Redirect(Url.Action("MyAccount", "Home") + "#appointments");
    }


    public IActionResult ContactUs()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}

