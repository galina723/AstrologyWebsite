using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/User")]
    [Authorize(Roles = "Admin")]
    public class UserController : Controller
    {
        private readonly AstrologyDatabaseContext context;

        public UserController(AstrologyDatabaseContext context)
        {
            this.context = context;
        }

        [HttpGet("Users")]
        public async Task<IActionResult> Users()
        {
            var users = await (from user in context.Users
                               join userRole in context.UserRoles on user.Id equals userRole.UserId
                               join role in context.Roles on userRole.RoleId equals role.Id
                               where role.Name == "User"
                               select user).ToListAsync();
            return View(users);
        }

        [HttpPost("ChangeStatus/{id}")]
        public IActionResult ChangeStatus(string id, [FromForm] AccountStatus newStatus)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["ToastMessage"] = "User not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            user.Status = newStatus;
            context.SaveChanges();

            TempData["ToastMessage"] = "User status updated successfully!";
            TempData["ToastType"] = "success";
            return RedirectToAction("Users");
        }

        [HttpPost("DeleteUser")]
        public IActionResult DeleteUser(string id)
        {
            var user = context.Users.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                TempData["ToastMessage"] = "User not found.";
                TempData["ToastType"] = "error";
                return NotFound();
            }

            user.IsDeleted = 1;
            context.SaveChanges();

            TempData["ToastMessage"] = "User deleted successfully!";
            TempData["ToastType"] = "success";

            return RedirectToAction("Users");
        }
    }
}
