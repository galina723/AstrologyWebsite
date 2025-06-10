using AstrologyWebsite.HoroscropServices;
using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers
{
    public class HoroscopeController : Controller
    {
        private readonly HoroscopeService _horoscopeService = new HoroscopeService();

        public async Task<ActionResult> Daily(string sign)
        {
            if (string.IsNullOrEmpty(sign))
            {
                return View("Error");
            }

            var horoscopeResult = await _horoscopeService.GetDailyHoroscopeAsync(sign);

            // If there's no horoscope, show the error message
            if (string.IsNullOrEmpty(horoscopeResult.Horoscope))
            {
                ViewBag.Horoscope = "Unable to fetch horoscope.";
            }
            else
            {
                ViewBag.Horoscope = horoscopeResult.Horoscope;
            }

            ViewBag.Sign = sign;
            return View();
        }
    }
}
