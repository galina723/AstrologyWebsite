using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers
{
    public class DashboardAnalytics : Controller
    {
        public IActionResult Index()
        {
            return View("~/Views/Admin/DashboardAnalytics/Index.cshtml");

        }
    }
}