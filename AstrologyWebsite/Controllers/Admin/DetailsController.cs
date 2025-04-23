using Microsoft.AspNetCore.Mvc;

namespace AstrologyWebsite.Controllers.Admin
{
    public class DetailsController : Controller
    {
        public IActionResult DashboardAnalytics()
        {
            return View("~/Views/Admin/Details/DashboardAnalytics.cshtml");

        }
    }
}