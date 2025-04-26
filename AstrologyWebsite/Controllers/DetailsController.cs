using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers
{
    public class DetailsController : Controller
    {
        public IActionResult Astrology()
        {
            return View();
        }
        public IActionResult Blog()
        {
            return View();
        }
        public IActionResult Service()
        {
            return View();
        }

        public IActionResult BlogDetail()
        {
            return View();
        }
       
    }
}
