using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Zodiac")]
    public class ZodiacController : Controller
    {
        private readonly AstrologyDatabaseContext context;

        public ZodiacController(AstrologyDatabaseContext context)
        {
            this.context = context;
        }

        [HttpGet("Zodiacs")]
        public IActionResult Zodiacs()
        {
            var zodiacs = context.Zodiacs.ToList();
            return View(zodiacs);
        }
        [HttpGet("CreateZodiac")]
        public IActionResult CreateZodiac()
        {
            return View();
        }

        [HttpPost("CreateZodiac")]
        public IActionResult AddZodiac(ZodiacDTO zodiac)
        {
            if (ModelState.IsValid)
            {
                Zodiac newZodiac = new Zodiac()
                {
                    Name = zodiac.Name,
                    Symbol = zodiac.Symbol,
                    Modality = zodiac.Modality,
                    StartDate = zodiac.StartDate,
                    EndDate = zodiac.EndDate,
                    Element = zodiac.Element,
                    Avatar = SaveImageIfNotExists(zodiac.Avatar),
                    Description = zodiac.Description
                };

                context.Zodiacs.Add(newZodiac);

                context.SaveChanges();
                TempData["ToastMessage"] = "Zodiac created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Zodiacs");
            }

            return View();
        }

        [HttpGet("EditZodiac/{id}")]
        public IActionResult EditZodiac(int? id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var zodiac = context.Zodiacs.Find(id);

            if (zodiac == null)
            {
                return RedirectToAction("Zodiac");
            }


            ZodiacDTO newZodiac = new ZodiacDTO()
            {
                Name = zodiac.Name,
                Symbol = zodiac.Symbol,
                Modality = zodiac.Modality,
                StartDate = zodiac.StartDate,
                EndDate = zodiac.EndDate,
                Element = zodiac.Element,
                AvatarUrl = zodiac.Avatar,
                Description = zodiac.Description
            };

            return View(newZodiac);
        }

        [HttpPost("EditZodiac/{id}")]
        public IActionResult EditZodiac(int id, ZodiacDTO newZodiacItem)
        {
            if (ModelState.IsValid)
            {

                var zodiac = context.Zodiacs.Find(id);

                zodiac.Name = newZodiacItem.Name;
                zodiac.Symbol = newZodiacItem.Symbol;
                zodiac.Modality = newZodiacItem.Modality;
                zodiac.StartDate = newZodiacItem.StartDate;
                zodiac.EndDate = newZodiacItem.EndDate;
                zodiac.Element = newZodiacItem.Element;
                if (newZodiacItem.Avatar != null)
                {
                    zodiac.Avatar = SaveImageIfNotExists(newZodiacItem.Avatar);
                }
                zodiac.Description = newZodiacItem.Description;
                TempData["ToastMessage"] = "Zodiac updated successfully!";
                TempData["ToastType"] = "success";
                context.SaveChanges();

                return RedirectToAction("Zodiacs");
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeleteZodiac(int id)
        {
            if (ModelState.IsValid)
            {
                var zodiac = context.Zodiacs.Find(id);
                if (zodiac != null)
                {
                    context.Zodiacs.Remove(zodiac);
                    context.SaveChanges(true);

                    TempData["ToastMessage"] = "Zodiac deleted successfully!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("Zodiacs");
                }
            }
            TempData["ToastMessage"] = "Zodiac not found.";
            TempData["ToastType"] = "error";
            return View();
        }
        private string SaveImageIfNotExists(IFormFile imageFile)
        {
            if (imageFile == null) return null;

            string uploadsFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/image");
            Directory.CreateDirectory(uploadsFolder);

            string filePath = Path.Combine(uploadsFolder, imageFile.FileName);

            if (!System.IO.File.Exists(filePath))
            {
                using var stream = new FileStream(filePath, FileMode.Create);
                imageFile.CopyTo(stream);
            }

            return "/image/" + imageFile.FileName;
        }
    }
}
