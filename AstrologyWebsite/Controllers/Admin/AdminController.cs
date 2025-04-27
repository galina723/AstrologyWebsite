using Microsoft.AspNetCore.Mvc;

public class AdminController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult DashboardAnalytics()
    {
        return View("~/Views/Admin/DashboardAnalytics.cshtml");

    }
}