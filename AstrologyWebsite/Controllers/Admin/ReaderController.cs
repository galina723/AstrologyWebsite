using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Controllers.Admin
{
    public class ReaderController : Controller
    {

        private readonly AstrologyDatabaseContext _context;
        public ReaderController( AstrologyDatabaseContext context)
        {
            _context = context;
        }

        public IActionResult Readers()
        {
            var readers = _context.AstroUsers.ToList();

            //List<AstroUser> listReader = new List<AstroUser>();

            //foreach (AstroUser reader in readers)
            //{
            //    listReader.Add(new AstroUser
            //    {
            //        Id = reader.Id,
            //        FullName = reader.FullName,
            //        Email = reader.Email,
            //        PhoneNumber = reader.PhoneNumber,
            //        Status = 1,
            //        Gender = reader.Gender,
            //        IsDeleted = 0,
            //    });
            //}

            return View(readers);
        }
        public IActionResult CreateReader()
        {
            return View();
        }

        public IActionResult AddReader(ReaderDTO readerDto)
        {
            if (ModelState.IsValid)
            {
                AstroUser newReader = new AstroUser()
                {
                    FullName = readerDto.FullName,
                    Gender = readerDto.Gender,
                    PhoneNumber = readerDto.PhoneNumber.ToString(),
                    Email = readerDto.Email,
                    Dob = readerDto.Dob,
                    IsDeleted = 0,
                    Status = 1,
                    RoleId = 1,
                    Password = readerDto.Password,
                };

                _context.AstroUsers.Add(newReader);

                _context.SaveChanges();

                return RedirectToAction("Readers");
            }

            return View("~/Views/Admin/Readers/Readers.cshtml");
        }
        public IActionResult EditReader(string? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var reader = _context.Users.Find(id);

            if (reader == null)
            {
                return RedirectToAction("Readers");
            }


            ReaderDTO newReader = new ReaderDTO()
            {
                Id = reader.Id,
                FullName = reader.FullName,
                Email = reader.Email,
                PhoneNumber = reader.PhoneNumber,
                Status = 1,
                Gender = reader.Gender,
                IsDeleted = 0,
                Dob = reader.Dob,
                Password = reader.Password,
            };
            return View("~/Views/Admin/Readers/EditReader.cshtml", newReader);
        }

        [HttpPost]
        public IActionResult EditReader(string id, ReaderDTO readerDto)
        {
            if (ModelState.IsValid)
            {

                var reader = _context.Users.Find(id);

                reader.FullName = readerDto.FullName;
                reader.Gender = readerDto.Gender;
                reader.PhoneNumber = readerDto.PhoneNumber.ToString();
                reader.Email = readerDto.Email;
                reader.Dob = readerDto.Dob;
                reader.Password = readerDto.Password;


                _context.SaveChanges();

                return RedirectToAction("Readers");
            }

            return View("~/Views/Admin/Readers/Readers.cshtml");
        }

        [HttpPost]
        public IActionResult DeleteReader(string id)
        {

            if (ModelState.IsValid)
            {
                var user = _context.Users.Find(id);

                if (user != null)
                {
                    _context.Users.Remove(user);
                    
                    _context.SaveChanges(true);

                    return RedirectToAction("Readers");
                }
            }

            return View("~/Views/Admin/Readers/Readers.cshtml");
        }

    }
}
