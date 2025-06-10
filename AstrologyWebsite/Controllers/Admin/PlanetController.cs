using AstrologyWebsite.DTOs;
using AstrologyWebsite.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers.Admin
{
    [Route("Admin/Planet")]
    [Authorize(Roles ="Admin")]
    public class PlanetController : Controller
    {
        private readonly AstrologyDatabaseContext context;

        public PlanetController(AstrologyDatabaseContext context)
        {
            this.context = context;
        }
        [HttpGet("Planets")]
        public IActionResult Planets()
        {
            var planets = context.Planets.ToList();


            return View( planets);
        }

        [HttpGet("CreatePlanet")]
        public IActionResult CreatePlanet()
        {
            return View();
        }

        [HttpPost("CreatePlanet")]
        public IActionResult AddPlanet(PlanetDTO planet)
        {
            if (ModelState.IsValid)
            {
                Planet newPlanet = new Planet()
                {
                    Name = planet.Name,
                    Symbol = planet.Symbol,
                    Description = planet.Description,
                };
                string avatar = SaveImageIfNotExists(planet.Avatar);
                newPlanet.Avatar = avatar;

                context.Planets.Add(newPlanet);

                context.SaveChanges();
                TempData["ToastMessage"] = "Planet created successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Planets");
            }

            return View();
        }

        [HttpGet("EditPlanet/{id}")]
        public IActionResult EditPlanet(int id)
        {
            if (id == null)
            {
                return BadRequest();
            }

            var planet = context.Planets.Find(id);

            if (planet == null)
            {
                return RedirectToAction("Planet");
            }


            PlanetDTO newPlanet = new PlanetDTO()
            {
                Name = planet.Name,
                Symbol = planet.Symbol,
                Description = planet.Description,
                AvatarURL = planet.Avatar
            };

            return View(newPlanet);
        }

        [HttpPost("EditPlanet/{id}")]
        public IActionResult EditPlanet(int id, PlanetDTO newPlanet)
        {
            if (ModelState.IsValid)
            {

                var planet = context.Planets.Find(id);

                planet.Name = newPlanet.Name;
                planet.Symbol = newPlanet.Symbol;
                planet.Description = newPlanet.Description;
                if (newPlanet.Avatar != null)
                {
                    planet.Avatar = SaveImageIfNotExists(newPlanet.Avatar);
                }

                context.SaveChanges();
                TempData["ToastMessage"] = "Planet updated successfully!";
                TempData["ToastType"] = "success";
                return RedirectToAction("Planets");
            }

            return View();
        }

        [HttpPost]
        public IActionResult DeletePlanet(int id)
        {

            if (ModelState.IsValid)
            {
                var planet = context.Planets.Find(id);
                if (planet != null)
                {
                    context.Planets.Remove(planet);
                    context.SaveChanges(true);

                    TempData["ToastMessage"] = "Planet deleted successfully!";
                    TempData["ToastType"] = "success";
                    return RedirectToAction("Planets");
                }
            }
            TempData["ToastMessage"] = "Planet not found.";
            TempData["ToastType"] = "error";

            return View("~/Views/Admin/Planets/Planets.cshtml");
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
