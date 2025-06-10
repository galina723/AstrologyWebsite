using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Reader")]
    public class ReaderController : Controller
    {
        private readonly AstrologyDatabaseContext context;
        private readonly UserManager<AstroUser> _userManager;
        public ReaderController(AstrologyDatabaseContext context, UserManager<AstroUser> userManager)
        {
            this.context = context;
            _userManager = userManager;
        }
        [HttpGet("Readers")]
        public async Task<IActionResult> Readers()
        {
            var reader = await (from user in context.Users
                                     join userRole in context.UserRoles on user.Id equals userRole.UserId
                                     join role in context.Roles on userRole.RoleId equals role.Id
                                     where role.Name == "TarotReader"
                                     select user).ToListAsync();

            return View(reader);
        }
        [HttpGet("CreateReader")]
        public IActionResult CreateReader()
        {
            return View();
        }
        [HttpPost("CreateReader")]
        public async Task<IActionResult> AddReader(ReaderDTO readerDto)
        {
            if (!ModelState.IsValid)
            {
                //foreach (var key in ModelState.Keys)
                //{
                //    var state = ModelState[key];
                //    foreach (var error in state.Errors)
                //    {
                //        Console.WriteLine($"ModelState error in '{key}': {error.ErrorMessage}");
                //    }
                //}
                return View("CreateReader");
            }


            AstroUser newReader = new AstroUser()
            {
                FullName = readerDto.FullName,
                Gender = readerDto.Gender,
                PhoneNumber = readerDto.PhoneNumber.ToString(),
                Email = readerDto.Email,
                UserName = readerDto.Email,
                Dob = readerDto.Dob,
                Experience = readerDto.Experience,
                IsDeleted = 0,
                Status = AccountStatus.Active,
                CreatedDate = DateTime.UtcNow,
                EmailConfirmed = true
            };

            var result = await _userManager.CreateAsync(newReader, readerDto.Password);

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newReader, "TarotReader");
                TempData["ToastMessage"] = "Reader created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Readers");
            }

            return View();
        }
        [HttpGet("EditReader/{id}")]
        public IActionResult EditReader(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var reader = context.Users.Find(id);

            if (reader == null)
            {
                return RedirectToAction("Readers");
            }


            EditReaderDto newReader = new EditReaderDto()
            {
                FullName = reader.FullName,
                Email = reader.Email,
                PhoneNumber = reader.PhoneNumber,
                Status = reader.Status,
                Gender = (int)reader.Gender,
                IsDeleted = 0,
                Dob = (DateOnly)reader.Dob,
                Experience = reader.Experience,
                
            };

            ViewBag.StatusList = Enum.GetValues(typeof(AccountStatus))
            .Cast<AccountStatus>()
            .Select(s => new SelectListItem
            {
                Value = ((int)s).ToString(),
                Text = s.ToString()
            }).ToList();

            return View(newReader);
        }

        [HttpPost("EditReader/{id}")]
        public async Task<IActionResult> EditReader(string id, EditReaderDto readerDto)
        {
            if (!ModelState.IsValid)
            {
                foreach (var key in ModelState.Keys)
                {
                    var state = ModelState[key];
                    foreach (var error in state.Errors)
                    {
                        Console.WriteLine($"ModelState error in '{key}': {error.ErrorMessage}");
                    }
                }
            }

                if (ModelState.IsValid)
            {

                var reader = context.Users.Find(id);

                reader.FullName = readerDto.FullName;
                reader.Gender = readerDto.Gender;
                reader.PhoneNumber = readerDto.PhoneNumber.ToString();
                reader.Dob = readerDto.Dob;
                reader.Status = (AccountStatus)readerDto.Status;
                reader.Experience = readerDto.Experience;

               await  context.SaveChangesAsync();
                TempData["ToastMessage"] = "Reader updated successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Readers");
            }

            return View();
        }

        [HttpPost("DeleteReader")]
        public IActionResult DeleteReader(string id)
        {
            if (ModelState.IsValid)
            {
                var user = context.Users.Find(id);
                if (user != null)
                {
                    user.IsDeleted = 1;
                    context.SaveChanges(true);

                    TempData["ToastMessage"] = "Reader deleted successfully!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("Readers");
                }
            }
            TempData["ToastMessage"] = "Reader not found.";
            TempData["ToastType"] = "error";

            return View();
        }
    }
}
