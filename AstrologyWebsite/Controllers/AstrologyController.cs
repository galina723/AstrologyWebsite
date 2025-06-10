using AstrologyWebsite.Models;
using AstrologyWebsite.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AstrologyWebsite.Controllers
{
    public class AstrologyController : Controller
    {
        private readonly AstrologyDatabaseContext _context;
        public AstrologyController(AstrologyDatabaseContext context)
        {
            _context = context;   
        }
        public async Task<IActionResult> Astrology()
        {
            var zodiac = await _context.Zodiacs.ToListAsync();
            var planets = await _context.Planets.ToListAsync();
            var banners = await _context.Banners.ToListAsync();
            ViewBag.Banners = banners;
            var model = new AstrologyViewModel
            {
                AllZodiac = zodiac,
                AllPlanets = planets,
                FireZodiac = zodiac.Where(z => z.Element == "Fire").ToList(),
                WaterZodiac = zodiac.Where(z => z.Element == "Water").ToList(),
                AirZodiac = zodiac.Where(z => z.Element == "Air").ToList(),
                EarthZodiac = zodiac.Where(z => z.Element == "Earth").ToList()
            };
            return View(model);
        }

        [HttpGet("Astrology/Planet/{id}")]
        public IActionResult PlanetDetail(int id)
        {
            var planet = _context.Planets.FirstOrDefault(p => p.Id == id);
            if (planet == null) return NotFound();
            return View("PlanetDetail", planet);
        }

        [HttpGet("Astrology/Zodiac/{id}")]
        public IActionResult ZodiacDetail(int id)
        {
            var zodiac = _context.Zodiacs.FirstOrDefault(z => z.Id == id);
            if (zodiac == null) return NotFound();
            return View("ZodiacDetail", zodiac);
        }
    }
}
